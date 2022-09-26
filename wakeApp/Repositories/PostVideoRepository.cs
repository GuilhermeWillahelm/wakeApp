using wakeApp.Models;
using wakeApp.Dtos;
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
        private readonly IChannelsRepository _channelsRepository;

        public PostVideoRepository(IUsersRepository usersRepository, IUploadService uploadService, IChannelsRepository channelsRepository)
        {
            _usersRepository = usersRepository;
            _uploadService = uploadService;
            _channelsRepository = channelsRepository;
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

        public PostVideoDto GetPostVideo(int? id)
        {
            if (id == null)
            {
                return null;
            }

            PostVideoDto postVideo = new PostVideoDto();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "PostVideos/" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                postVideo = JsonConvert.DeserializeObject<PostVideoDto>(data);
            }

            if (postVideo == null)
            {
                return null;
            }

            return postVideo;

        }

        public PostVideo CreatePostVideo(PostVideo postVideo, IFormFile fileImage, IFormFile fileVideo)
        {
            var channel = _channelsRepository.GetChannelById(_usersRepository.GetUserId());
            postVideo.VideoFile = _uploadService.UploadVideo(fileVideo);
            postVideo.ThumbImage = _uploadService.UploadImage(fileImage);
            postVideo.Posted = DateTime.Now;
            postVideo.UserId = _usersRepository.GetUserId();         
            postVideo.ChannelId = channel.Id;

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

        private static PostVideoDto ItemToDTO(PostVideo todoItem) =>
            new PostVideoDto
            {
                Id = todoItem.Id,
                Title = todoItem.Title,
                Description = todoItem.Description,
                Posted = todoItem.Posted,
                VideoFile = todoItem.VideoFile,
                ThumbImage = todoItem.ThumbImage,
                UserId = todoItem.UserId,
                ChannelId = todoItem.ChannelId,
                ChannelDto = new ChannelDto
                {
                    ChannelName = todoItem.Channel.ChannelName,
                    IconChannel = todoItem.Channel.IconChannel
                },
                LikeId = todoItem.LikeId,
                LikeDto = new LikeDto
                {
                    CountLike = todoItem.Like.CountLike,
                    CountDislike = todoItem.Like.CountDislike
                },
                CommentId = todoItem.CommentId,
                CommentDto = new CommentDto
                {
                    CommentText = todoItem.Comment.CommentText
                }

            };
    }
}
