#nullable disable
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using wakeApp.Data;
using wakeApp.Services;
using wakeApp.Repositories;
using wakeApp.Models;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace wakeApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly wakeAppContext _context;
        HttpClient _httpClient = new HttpClient() { BaseAddress = new Uri("https://localhost:7099/api/") };
        private readonly IUsersRepository _usersRepository;

        public UsersController(wakeAppContext context, IUsersRepository usersRepository)
        {
            _context = context;
            _usersRepository = usersRepository;
        }

        // GET: Users
        public IActionResult Index()
        {
            ViewBag.NameLogin = _usersRepository.GetUserName();
            ViewBag.UseID = _usersRepository.GetUserId();
            return RedirectToRoute(new { controller = "PostVideos", action = "Index"});
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.NameLogin = _usersRepository.GetUserName();
            ViewBag.UseID = _usersRepository.GetUserId();
            if (id == null)
            {
                return NotFound();
            }

            var user = _usersRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewBag.NameLogin = _usersRepository.GetUserName();
            ViewBag.UseID = _usersRepository.GetUserId();
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,UserName,FullName,Email,Password")] User user)
        {
            user = _usersRepository.CreateUser(user);
            return View(user);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public ActionResult Login([Bind("UserName,Password")] UserLogin login )
        {
            var userLogin = _usersRepository.LoginUser(login);

            if (userLogin == false)
            {
                return RedirectToRoute(new { controller = "PostVideos", action = "Index"});
            }

            return View();
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.NameLogin = _usersRepository.GetUserName();
            ViewBag.UseID = _usersRepository.GetUserId();
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,FullName,Email,Password")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            user = _usersRepository.EditUser(id, user);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [Authorize]
        public IActionResult Logoff()
        {
            _usersRepository.Logoff();
            return RedirectToRoute(new { controller = "PostVideos", action = "Index" });
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.NameLogin = _usersRepository.GetUserName();
            ViewBag.UseID = _usersRepository.GetUserId();
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = _usersRepository.DeleteUser(id);

            if (!user)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
