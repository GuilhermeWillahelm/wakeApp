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
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "Channels").Result;

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                channels = JsonConvert.DeserializeObject<List<Channel>>(data);
            }

            return channels;
        }

        public ChannelDto GetChannelById(int? id)
        {
            ChannelDto channel = new ChannelDto();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "Channels/GetChannelByUser/" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                channel = JsonConvert.DeserializeObject<ChannelDto>(data);
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
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "PostVideos/GetPostVideoById/" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                postVideos = JsonConvert.DeserializeObject<List<PostVideoDto>>(data);
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

            HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "Channels", content).Result;

            if (response.IsSuccessStatusCode)
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

            HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "Channels", content).Result;
            if (response.IsSuccessStatusCode)
            {
                return null;
            }

            return channel;
        }

        public bool DeleteChannel(int id)
        {
            HttpResponseMessage response = _httpClient.DeleteAsync(_httpClient.BaseAddress + "Channels/" + id).Result;

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }
    }
}
