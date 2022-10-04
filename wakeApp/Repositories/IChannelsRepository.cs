using wakeApp.Dtos;
using wakeApp.Models;

namespace wakeApp.Repositories
{
    public interface IChannelsRepository
    {
        List<Channel> GetAllChannels();
        ChannelDto GetChannelById(int? id);
        List<PostVideoDto> GetAllVideosPerChannel(int? id);
        Channel CreateChannel(Channel channel, IFormFile fileBanner, IFormFile fileIcon);
        Channel EditChannel(int id, Channel channel, IFormFile fileBanner);
        bool DeleteChannel(int id);
    }
}
