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
using wakeApp.Repositories;
using Newtonsoft.Json;
using wakeApp.Data;
using wakeApp.Models;

namespace wakeApp.Controllers
{
    public class ChannelsController : Controller
    {
        HttpClient _httpClient = new HttpClient() { BaseAddress = new Uri("https://localhost:7099/api/") };
        private readonly IChannelsRepository _channelsRepository;
        private readonly IPostVideoRepository _postVideoRepository;
        private readonly IUsersRepository _usersRepository;

        public ChannelsController(IChannelsRepository channelsRepository, IPostVideoRepository postVideoRepository, IUsersRepository usersRepository)
        {
            _channelsRepository = channelsRepository;
            _postVideoRepository = postVideoRepository;
            _usersRepository = usersRepository;
        }

        // GET: Channels
        public IActionResult Index()
        {
            ViewBag.NameLogin = _usersRepository.GetUserName();
            ViewBag.UseID = _usersRepository.GetUserId();
            var channels = _channelsRepository.GetAllChannels();
            return View(channels);
        }

        // GET: Channels/Details/5
        public IActionResult Details(int? id)
        {
            ViewBag.NameLogin = _usersRepository.GetUserName();
            ViewBag.UseID = _usersRepository.GetUserId();

            if (id == null)
            {
                return NotFound();
            }
            
            var channel = _channelsRepository.GetChannelById(id);

            if (channel == null)
            {
                return NotFound();
            }

            return View(channel);
        }

        public IActionResult Videos()
        {
            var id = _usersRepository.GetUserId();
            if (id == null)
            {
                return NotFound();
            }

            var videos = _postVideoRepository.GetAllVideosPerChannel(id);

            if (videos == null)
            {
                return NotFound();
            }

            return View(videos.ToList());
        }

        // GET: Channels/Create
        public IActionResult Create()
        {
            ViewBag.NameLogin = _usersRepository.GetUserName();
            ViewBag.UseID = _usersRepository.GetUserId();
            return View();
        }

        // POST: Channels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,ChannelName,ChannelDescription,CreatedChannel")] Channel channel, [Bind("BannerChannel")] IFormFile BannerChannel)
        {
            channel = _channelsRepository.CreateChannel(channel, BannerChannel);
            return View(channel);
        }

        // GET: Channels/Edit/5
        public IActionResult Edit(int? id)
        {
            ViewBag.NameLogin = _usersRepository.GetUserName();
            ViewBag.UseID = _usersRepository.GetUserId();
            if (id == null)
            {
                return NotFound();
            }

            var channel = _channelsRepository.GetChannelById(id);

            if (channel == null)
            {
                return NotFound();
            }

            return View(channel);
        }

        // POST: Channels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,ChannelName,ChannelDescription")] Channel channel, [Bind("BannerImage")] IFormFile BannerImage)
        {
            if (id != channel.Id)
            {
                return NotFound();
            }

            channel = _channelsRepository.EditChannel(id, channel, BannerImage);

            if (channel == null)
            {
                return BadRequest();
            }

            return View(channel);
        }

        // GET: Channels/Delete/5
        public IActionResult Delete(int? id)
        {
            ViewBag.NameLogin = _usersRepository.GetUserName();
            ViewBag.UseID = _usersRepository.GetUserId();
            if (id == null)
            {
                return NotFound();
            }

            var channel = _channelsRepository.GetChannelById(id);

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
            var channelReturn = _channelsRepository.DeleteChannel(id);

            if(channelReturn == false)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
