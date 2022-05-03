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

namespace wakeApp.Controllers
{
    public class PostVideosController : Controller
    {
        private readonly IPostVideoRepository _repository;
        HttpClient _httpClient = new HttpClient() { BaseAddress = new Uri("https://localhost:7099/api/") };
        private readonly IUsersRepository _usersRepository;

        public PostVideosController(IPostVideoRepository repository, IUsersRepository usersRepository)
        {
            _repository = repository;
            _usersRepository = usersRepository;
        }
        // GET: PostVideos
        public IActionResult Index(string? searchString)
        {
            ViewBag.NameLogin = _usersRepository.GetUserName();
            var videos = _repository.GetAllVideos(searchString);
            
            return View(videos.ToList());
        }

        // GET: PostVideos/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.NameLogin = _usersRepository.GetUserName();
            var postVideo = _repository.GetPostVideo(id); 
            return View(postVideo);
        }

        // GET: PostVideos/Create
        public IActionResult Create()
        {
            ViewBag.NameLogin = _usersRepository.GetUserName();
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
            return View(video);
        }

        // GET: PostVideos/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
