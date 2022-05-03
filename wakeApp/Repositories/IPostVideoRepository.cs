using wakeApp.Models;

namespace wakeApp.Repositories
{
    public interface IPostVideoRepository
    {
        List<PostVideo> GetAllVideos(string? stringSearch);
    }
}
