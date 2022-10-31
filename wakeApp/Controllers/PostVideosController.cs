#nullable disable
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using wakeApp.Data;
using wakeApp.Models;
using wakeApp.Services;
using wakeApp.Repositories;
using wakeApp.Dtos;

namespace wakeApp.Controllers
{
    public class PostVideosController : Controller
    {
        private readonly IPostVideoRepository _repository;
        HttpClient _httpClient = new HttpClient() { BaseAddress = new Uri("https://localhost:7099/api/") };
        private readonly IUsersRepository _usersRepository;
        ViewModel viewModel = new ViewModel();
        public PostVideosController(IPostVideoRepository repository, IUsersRepository usersRepository)
        {
            _repository = repository;
            _usersRepository = usersRepository;
        }
        // GET: PostVideos
        public IActionResult Index(string? searchString)
        {
            ViewBag.NameLogin = _usersRepository.GetUserName();
            ViewBag.UseID = _usersRepository.GetUserId();
            viewModel.PostVideoDtos = _repository.GetAllVideos(searchString);

            if(viewModel.PostVideoDtos == null)
            {
                return View("Error");
            }
            
            return View(viewModel);
        }

        // GET: PostVideos/Details/5
        public ActionResult Details(int? id)
       {
            ViewBag.NameLogin = _usersRepository.GetUserName();
            ViewBag.UseID = _usersRepository.GetUserId();
            viewModel.PostVideoDto = _repository.GetPostVideo(id);
            viewModel.CountLike = _repository.GetLikesPerVideos(viewModel.PostVideoDto.Id);
            viewModel.CommentDtos = _repository.GetCommentsPerVideos(viewModel.PostVideoDto.Id);
            viewModel.CountFollowers = _repository.GetFollowersPerChannel(viewModel.PostVideoDto.ChannelId);

            return View(viewModel);
        }

        // GET: PostVideos/Create
        public IActionResult Create()
        {
            ViewBag.NameLogin = _usersRepository.GetUserName();
            ViewBag.UseID = _usersRepository.GetUserId();
            return View();
        }

        // POST: PostVideos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,Title,Description")] PostVideo postVideo, [Bind("FileVideo")] IFormFile FileVideo, [Bind("FileImage")] IFormFile FileImage)
        {
            var video = _repository.CreatePostVideo(postVideo, FileImage, FileVideo);

            if(video == null)
            {
                RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateLike([Bind("Id,PostId,ChannelId,UserId")] EvaluationDto evaluation)
        {
            evaluation.CountLike = 1;
            var id = evaluation.PostId;
            evaluation = _repository.AddLike(evaluation);

            if (evaluation == null)
            {
                RedirectToAction(nameof(Index));
            }

            return Redirect("/PostVideos/Details/" + id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment([Bind("Id,CommentText,UserId,ChannelId,PostId")] CommentDto commentDto)
        {
            commentDto.Flag = true;
            var id = commentDto.PostId;
            commentDto = _repository.AddComment(commentDto);

            if (commentDto == null)
            {
                RedirectToAction(nameof(Index));
            }
            return Redirect("/PostVideos/Details/" + id);
        }

        // GET: PostVideos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.NameLogin = _usersRepository.GetUserName();
            ViewBag.UseID = _usersRepository.GetUserId();
            if (id == null)
            {
                return NotFound();
            }

            var postVideo = _repository.GetPostVideo(id);

            if (postVideo == null)
            {
                return NotFound();
            }

            return View(postVideo);
        }

        // POST: PostVideos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description")] PostVideo postVideo, [Bind("FileVideo")] IFormFile FileVideo, [Bind("FileImage")] IFormFile FileImage)
        {
            var video = _repository.EditVideo(id, postVideo, FileVideo, FileImage);

            if (video == null)
            {
                RedirectToAction(nameof(Index));
            }

            return View(postVideo);
        }

        // GET: PostVideos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.NameLogin = _usersRepository.GetUserName();
            if (id == null)
            {
                return NotFound();
            }

            var postVideo = _repository.GetPostVideo(id);

            if (postVideo == null)
            {
                return NotFound();
            }

            return View(postVideo);
        }

        // POST: PostVideos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var videoDelete = _repository.DeleteVideo(id);

            if (!videoDelete)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
