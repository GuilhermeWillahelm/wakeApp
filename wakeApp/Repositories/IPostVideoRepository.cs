using wakeApp.Models;

namespace wakeApp.Repositories
{
    public interface IPostVideoRepository
    {
        List<PostVideo> GetAllVideos(string? stringSearch);
        List<PostVideo> GetAllVideosPerChannel(int? id);
        PostVideo GetPostVideo(int? id);
        PostVideo CreatePostVideo(PostVideo postVideo, IFormFile fileImage, IFormFile fileVideo);
        PostVideo EditVideo(int id, PostVideo postVideo, IFormFile fileImage, IFormFile fileVideo);
        bool DeleteVideo(int id);
    }
}
