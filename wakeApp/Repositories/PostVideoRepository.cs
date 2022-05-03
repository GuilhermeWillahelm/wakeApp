using wakeApp.Data;
using wakeApp.Models;
using Microsoft.EntityFrameworkCore;
using wakeApp.Services;
using Newtonsoft.Json;

namespace wakeApp.Repositories
{
    public class PostVideoRepository : IPostVideoRepository
    {
        private readonly wakeAppContext _context;
        HttpClient _httpClient = new HttpClient() { BaseAddress = new Uri("https://localhost:7099/api/") };
        private readonly IUserService _userService;

        public PostVideoRepository(wakeAppContext context)
        {
            _context = context;
        }

        public List<PostVideo> GetAllVideos(string? searchString)
        {
            List<PostVideo> videos = new List<PostVideo>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "PostVideos").Result;

            if (!String.IsNullOrEmpty(searchString))
            {
                response = _httpClient.GetAsync(_httpClient.BaseAddress + "PostVideos?searchString=" + searchString).Result;
            }

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                videos = JsonConvert.DeserializeObject<List<PostVideo>>(data);
            }

            return videos;
        }
    }
}
