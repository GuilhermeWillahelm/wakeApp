using wakeApp.Models;

namespace wakeApp.Repositories
{
    public interface IChannelsRepository
    {
        List<Channel> GetAllChannels();
        Channel GetChannelById(int? id);
        Channel CreateChannel(Channel channel, IFormFile fileBanner);
        Channel EditChannel(int id, Channel channel, IFormFile fileBanner);
        bool DeleteChannel(int id);
    }
}
