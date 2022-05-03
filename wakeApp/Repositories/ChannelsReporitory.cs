using Newtonsoft.Json;
using System.Text;
using wakeApp.Models;
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

        public Channel GetChannelById(int? id)
        {
            Channel channel = new Channel();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "Channels/" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                channel = JsonConvert.DeserializeObject<Channel>(data);
            }

            return channel;
        }

        public Channel CreateChannel(Channel channel, IFormFile fileBanner)
        {
            channel.ImageBanner = _uploadService.UploadImage(fileBanner);
            channel.UserId = _usersRepository.GetUserId();
            channel.FollwerId = 0;

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
