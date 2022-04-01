#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using wakeApp.Data;
using wakeApp.Models;

namespace wakeApp.Controllers
{
    public class PostVideosController : Controller
    {
        private readonly wakeAppContext _context;
        HttpClient _httpClient = new HttpClient() { BaseAddress = new Uri("https://localhost:7099/api/") };

        public PostVideosController(wakeAppContext context)
        {
            _context = context;
        }

        // GET: PostVideos
        public IActionResult Index()
        {
            ///var wakeAppContext = _context.PostVideos.Include(p => p.User);
            List<PostVideo> videos = new List<PostVideo>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "PostVideos").Result;

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                videos = JsonConvert.DeserializeObject<List<PostVideo>>(data);
            }

            return View(videos.ToList());
        }

        // GET: PostVideos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PostVideo postVideo = new PostVideo();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "PostVideos/" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                postVideo = JsonConvert.DeserializeObject<PostVideo>(data);
            }

            if (postVideo == null)
            {
                return NotFound();
            }

            return View(postVideo);
        }

        // GET: PostVideos/Create
        public IActionResult Create()
        {
            //ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "Id");
            return View();
        }

        // POST: PostVideos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,Title,Description")] PostVideo postVideo, [Bind("FileVideo")] IFormFile FileVideo, [Bind("FileImage")] IFormFile FileImage)
        {

            postVideo.VideoFile = UploadVideo(FileVideo);
            postVideo.ThumbImage = UploadImage(FileImage);
            postVideo.Posted = DateTime.Now;
            postVideo.UserId = 1;

            string data = JsonConvert.SerializeObject(postVideo);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "PostVideos/CreatePostVideo", content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(postVideo);
        }

        // GET: PostVideos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "Id", postVideo.UserId);

            if (id == null)
            {
                return NotFound();
            }

            PostVideo postVideo = new PostVideo();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "PostVideos/" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                postVideo = JsonConvert.DeserializeObject<PostVideo>(data);
            }

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
            if (id != postVideo.Id)
            {
                return NotFound();
            }

            postVideo.VideoFile = UploadVideo(FileVideo);
            postVideo.ThumbImage = UploadImage(FileImage);
            postVideo.Posted = DateTime.Now;
            postVideo.UserId = 1;

            string data = JsonConvert.SerializeObject(postVideo);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "PostVideos/UpdatePostVideo/" + id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(postVideo);
            //ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "Id", postVideo.UserId);
        }

        // GET: PostVideos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PostVideo postVideo = new PostVideo();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "PostVideos/" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                postVideo = JsonConvert.DeserializeObject<PostVideo>(data);
            }

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

            HttpResponseMessage response = _httpClient.DeleteAsync(_httpClient.BaseAddress + "PostVideos/" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PostVideoExists(int id)
        {
            return _context.PostVideos.Any(e => e.Id == id);
        }
        private string UploadImage(IFormFile file)
        {
            Account account = new Account("imagedpy", "882864429614789", "J0ISV-xrcX_pod7fhdyLSJ06Gl4");
            Cloudinary cloudinary = new Cloudinary(account);
            var filename = file.FileName;

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription($@"D:/Computador/Images/Desenhos/{filename}"),
                PublicId = filename.Replace("D:/Computador/Images/Desenhos/", "").Replace(".jpg", ""),
                UploadPreset = "h2necen7",
                Folder = "thumbnails",
                ImageMetadata = true,

            };
            var uploadResult = cloudinary.Upload(uploadParams);

            return uploadResult.SecureUrl.OriginalString;
        }

        private string UploadVideo(IFormFile file)
        {
            Account account = new Account("imagedpy", "882864429614789", "J0ISV-xrcX_pod7fhdyLSJ06Gl4");
            Cloudinary cloudinary = new Cloudinary(account);
            var filename = file.FileName;

            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription($@"D:/Computador/Images/Desenhos/{filename}"),
                PublicId = filename.Replace("D:/Computador/Images/Desenhos/","").Replace(".mp4", ""),
                UploadPreset = "w4rctdor",
                Folder = "videos",
                ImageMetadata = true,

            };
            var uploadResult = cloudinary.Upload(uploadParams);

            return uploadResult.SecureUrl.OriginalString;
        }
    }
}
