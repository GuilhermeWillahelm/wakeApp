using wakeApp.Models;
using wakeApp.Dtos;
using wakeApp.Services;

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

        public List<PostVideoDto> GetAllVideos(string? searchString)
        {
            List<PostVideoDto> videos = new List<PostVideoDto>();
            try
            {
                var response = _httpClient.GetFromJsonAsync<List<PostVideoDto>>(_httpClient.BaseAddress + "PostVideos");
                response.Wait();

                if (!String.IsNullOrEmpty(searchString))
                {
                    videos = response.Result;
                }

                if (response.Status == TaskStatus.RanToCompletion)
                {
                    videos = response.Result;
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
            var response = _httpClient.GetFromJsonAsync<PostVideoDto>(_httpClient.BaseAddress + "PostVideos/" + id);
            response.Wait();


            if (response.Status == TaskStatus.RanToCompletion)
            {
                postVideo = response.Result;
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

            //string data = JsonConvert.SerializeObject(postVideo);
            //StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = _httpClient.PostAsJsonAsync<PostVideo>(_httpClient.BaseAddress + "PostVideos/CreatePostVideo", postVideo);
            response.Wait();
            if (response.Status == TaskStatus.RanToCompletion)
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

            //string data = JsonConvert.SerializeObject(postVideo);
            //StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = _httpClient.PostAsJsonAsync<PostVideo>(_httpClient.BaseAddress + "PostVideos/UpdatePostVideo/", postVideo);
            response.Wait();

            if (response.Status == TaskStatus.RanToCompletion)
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

        public List<PostVideoDto> GetAllVideosPerChannel(int? id)
        {
            if (id == null)
            {
                return null;
            }

            List<PostVideoDto> postVideos = new List<PostVideoDto>();
            var response = _httpClient.GetFromJsonAsync<List<PostVideoDto>>(_httpClient.BaseAddress + "PostVideos/GetPostVideoById/" + id);
            response.Wait();

            if (response.Status == TaskStatus.RanToCompletion)
            {
                postVideos = response.Result;
            }

            if (postVideos == null)
            {
                return null;
            }

            return postVideos;
        }

        public int GetFollowersPerChannel(int? channelId)
        {
            if (channelId == null)
            {
                return 0;
            }

            int aux = 0;
            List<Follower> followers = new List<Follower>();
            var response = _httpClient.GetFromJsonAsync<List<Follower>>(_httpClient.BaseAddress + "Followers/GetFollowersPerChannel/" + channelId);
            response.Wait();

            if (response.Status == TaskStatus.RanToCompletion)
            {
                followers = response.Result;
            }

            if (followers == null)
            {
                return 0;
            }

            foreach (var value in followers)
            {
                aux = value.CountFollows;
            }

            return aux;
        }

        public int GetLikesPerVideos(int? idVideo)
        {
            if (idVideo == null)
            {
                return 0;
            }

            var aux = 0;
            List<Evaluation> evaluations = new List<Evaluation>();
            var response = _httpClient.GetFromJsonAsync<List<Evaluation>>(_httpClient.BaseAddress + "Evaluations/GetLikesPerVideo/" + idVideo);
            response.Wait();

            if (response.Status == TaskStatus.RanToCompletion)
            {
                evaluations = response.Result;
            }

            if(evaluations == null)
            {
                return 0;
            }

            foreach (var value in evaluations)
            {
                aux += value.CountLike;
            }

            return aux;
        }

        public EvaluationDto AddLike(EvaluationDto evaluation)
        {
            if(evaluation == null)
            {
                return null;
            }

            evaluation.UserId = _usersRepository.GetUserId();
            //string data = JsonConvert.SerializeObject(evaluation);
            //StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = _httpClient.PostAsJsonAsync<EvaluationDto>(_httpClient.BaseAddress + "Evaluations/", evaluation);
            response.Wait();

            if (response.Status == TaskStatus.RanToCompletion)
            {
                return null;
            }

            return evaluation;
        }

        public Evaluation UpdateLike(int? idLike, int? idVideo, Evaluation like)
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
                    Id = todoItem.Channel.Id,
                    ChannelName = todoItem.Channel.ChannelName,
                    IconChannel = todoItem.Channel.IconChannel,
                    CountFollows = todoItem.Channel.CountFollows,
                }
            };

    }
}
