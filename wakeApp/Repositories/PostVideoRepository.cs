using wakeApp.Models;
using wakeApp.Services;
using Newtonsoft.Json;
using System.Text;

namespace wakeApp.Repositories
{
    public class PostVideoRepository : IPostVideoRepository
    {
        HttpClient _httpClient = new HttpClient() { BaseAddress = new Uri("https://localhost:7099/api/") };
        private readonly IUsersRepository _usersRepository;
        private readonly IUploadService _uploadService;

        public PostVideoRepository(IUsersRepository usersRepository, IUploadService uploadService)
        {
            _usersRepository = usersRepository;
            _uploadService = uploadService;
        }

        public List<PostVideo> GetAllVideos(string? searchString)
        {
            List<PostVideo> videos = new List<PostVideo>();
            try
            {
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
            catch (Exception ex)
            {
                return null;
            }
            return videos;
        }

        public PostVideo GetPostVideo(int? id)
        {
            if (id == null)
            {
                return null;
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
                return null;
            }

            return postVideo;

        }

        public PostVideo CreatePostVideo(PostVideo postVideo, IFormFile fileImage, IFormFile fileVideo)
        {
            postVideo.VideoFile = _uploadService.UploadVideo(fileVideo);
            postVideo.ThumbImage = _uploadService.UploadImage(fileImage);
            postVideo.Posted = DateTime.Now;
            postVideo.UserId = _usersRepository.GetUserId();

            string data = JsonConvert.SerializeObject(postVideo);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "PostVideos/CreatePostVideo", content).Result;
            if (response.IsSuccessStatusCode)
            {
                return null;
            }

            return postVideo;
        }

        public PostVideo EditVideo(int id, PostVideo postVideo, IFormFile fileImage, IFormFile fileVideo)
        {
            if (id != postVideo.Id)
            {
                return null;
            }

            postVideo.VideoFile = _uploadService.UploadVideo(fileVideo);
            postVideo.ThumbImage = _uploadService.UploadImage(fileImage);
            postVideo.Posted = DateTime.Now;
            postVideo.UserId = _usersRepository.GetUserId();

            string data = JsonConvert.SerializeObject(postVideo);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "PostVideos/UpdatePostVideo/" + id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return null;
            }

            return postVideo;
        }

        public bool DeleteVideo(int id)
        {
            HttpResponseMessage response = _httpClient.DeleteAsync(_httpClient.BaseAddress + "PostVideos/" + id).Result;

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }

        public List<PostVideo> GetAllVideosPerChannel(int? id)
        {
            if (id == null)
            {
                return null;
            }

            List<PostVideo> postVideos = new List<PostVideo>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "PostVideos/GetPostVideoById/" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                postVideos = JsonConvert.DeserializeObject<List<PostVideo>>(data);
            }

            if (postVideos == null)
            {
                return null;
            }

            return postVideos;
        }

        public List<Like> GetLikesPerVideos(int? idLike, int? idVideo)
        {
            if(idLike == null && idVideo == null)
            {
                return null;
            }

            List<Like> likes = new List<Like>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "" + idLike).Result;

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                likes = JsonConvert.DeserializeObject<List<Like>>(data);
            }

            if(likes == null)
            {
                return null;
            }

            return likes;
        }

        public Like AddLike(Like like)
        {
            if(like == null)
            {
                return null;
            }

            like.UserId = _usersRepository.GetUserId();
            string data = JsonConvert.SerializeObject(like);

            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "Like/", content).Result;
            if (response.IsSuccessStatusCode)
            {
                return null;
            }

            return like;

        }

        public Like UpdateLike(int? idLike, int? idVideo, Like like)
        {
            throw new NotImplementedException();
        }
    }
}
