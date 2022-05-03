#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using wakeApp.Services;
using Newtonsoft.Json;
using wakeApp.Data;
using wakeApp.Models;

namespace wakeApp.Controllers
{
    public class ChannelsController : Controller
    {
        private readonly wakeAppContext _context;
        HttpClient _httpClient = new HttpClient() { BaseAddress = new Uri("https://localhost:7099/api/") };
        private readonly IUserService _userService;

        public ChannelsController(wakeAppContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // GET: Channels
        public IActionResult Index()
        {
            List<Channel> channels = new List<Channel>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "Channels").Result;

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                channels = JsonConvert.DeserializeObject<List<Channel>>(data);
            }

            return View(channels.ToList());
        }

        // GET: Channels/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Channel channel = new Channel();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "Channels/" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                channel = JsonConvert.DeserializeObject<Channel>(data);
            }


            if (channel == null)
            {
                return NotFound();
            }

            return View(channel);
        }

        // GET: Channels/Create
        public IActionResult Create()
        {
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Channels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,ChannelName,ChannelDescription,CreatedChannel")] Channel channel, [Bind("BannerChannel")] IFormFile BannerChannel)
        {
            channel.ImageBanner = UploadImage(BannerChannel);
            channel.UserId = _userService.GetUserId();
            channel.FollwerId = 0;

            var data = JsonConvert.SerializeObject(channel);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "Channels", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", channel.UserId);
            return View(channel);
        }

        // GET: Channels/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Channel channel = new Channel();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "Channels/" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                channel = JsonConvert.DeserializeObject<Channel>(data);
            }

            if (channel == null)
            {
                return NotFound();
            }
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", channel.UserId);
            return View(channel);
        }

        // POST: Channels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,ChannelName,ChannelDescription")] Channel channel, [Bind("BannerImage")] IFormFile BannerImage)
        {
            if (id != channel.Id)
            {
                return NotFound();
            }

            channel.ImageBanner = UploadImage(BannerImage);
            channel.UserId = _userService.GetUserId();

            var data = JsonConvert.SerializeObject(channel);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "Channels", content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", channel.UserId);
            return View(channel);
        }

        // GET: Channels/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Channel channel = new Channel();
            HttpResponseMessage response = _httpClient.DeleteAsync(_httpClient.BaseAddress + "Channels/" + id).Result;

            if(response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                channel = JsonConvert.DeserializeObject<Channel>(data);
            }

            if (channel == null)
            {
                return NotFound();
            }

            return View(channel);
        }

        // POST: Channels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            HttpResponseMessage response = _httpClient.DeleteAsync(_httpClient.BaseAddress + "Channels/" + id).Result;

            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ChannelExists(int id)
        {
            return _context.Channels.Any(e => e.Id == id);
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

    }
}
