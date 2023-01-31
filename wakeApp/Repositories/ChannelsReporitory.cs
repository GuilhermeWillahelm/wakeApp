using Newtonsoft.Json;
using System.Text;
using wakeApp.Models;
using wakeApp.Dtos;
using wakeApp.Services;

namespace wakeApp.Repositories
{
    public class ChannelsReporitory : IChannelsRepository
    {
        HttpClient _httpClient = new HttpClient() { BaseAddress = new Uri("https://localhost:7099/api/") };
        private readonly IUsersRepository _usersRepository;
        private readonly IUploadService _uploadService;

        public ChannelsReporitory(IUsersRepository usersRepository, IUploadService uploadService)
        {
            _usersRepository = usersRepository;
            
            _uploadService = uploadService;
        }

        public List<Channel> GetAllChannels()
        {
            List<Channel> channels = new List<Channel>();
            var response = _httpClient.GetFromJsonAsync<List<Channel>>(_httpClient.BaseAddress + "Channels");
            response.Wait();

            if (response.Status == TaskStatus.RanToCompletion)
            {
                //var data = response.Content.ReadAsStringAsync().Result;
                channels = response.Result; ;
            }

            return channels;
        }

        public ChannelDto GetChannelById(int? id)
        {
            ChannelDto channel = new ChannelDto();
            var response = _httpClient.GetFromJsonAsync<ChannelDto>(_httpClient.BaseAddress + "Channels/GetChannelByUser/" + id);
            response.Wait();

            if (response.Status == TaskStatus.RanToCompletion)
            {
                //var data = response.Content.ReadAsStringAsync().Result;
                channel = response.Result;
            }


            return channel;
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
                //var data = response.Content.ReadAsStringAsync().Result;
                postVideos = response.Result;
            }

            if (postVideos == null)
            {
                return null;
            }

            return postVideos;
        }

        public Channel CreateChannel(Channel channel, IFormFile fileBanner, IFormFile fileIcon)
        {
            channel.ImageBanner = _uploadService.UploadImage(fileBanner);
            channel.IconChannel = _uploadService.UploadImage(fileIcon);
            channel.UserId = _usersRepository.GetUserId();

            var data = JsonConvert.SerializeObject(channel);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            //HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "Channels", content).Result;
            var response = _httpClient.PostAsJsonAsync<Channel>(_httpClient.BaseAddress + "Channels", channel);
            response.Wait();

            if (response.Status == TaskStatus.RanToCompletion)
            {
                return null;
            }

            return channel;
        }

        public Channel EditChannel(int id, Channel channel, IFormFile fileBanner)
        {
            channel.ImageBanner = _uploadService.UploadImage(fileBanner);
            channel.UserId = _usersRepository.GetUserId();

            var data = JsonConvert.SerializeObject(channel);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            var response = _httpClient.PutAsJsonAsync<Channel>(_httpClient.BaseAddress + "Channels", channel);
            response.Wait();

            if (response.Status == TaskStatus.RanToCompletion)
            {
                return null;
            }

            return channel;
        }

        public bool DeleteChannel(int id)
        {
            var response = _httpClient.DeleteAsync(_httpClient.BaseAddress + "Channels/" + id);
            response.Wait();

            if (response.Status != TaskStatus.RanToCompletion)
            {
                return false;
            }

            return true;
        }
    }
}
