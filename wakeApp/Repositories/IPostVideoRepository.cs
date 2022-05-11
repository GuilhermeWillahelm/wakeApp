using wakeApp.Models;

namespace wakeApp.Repositories
{
    public interface IPostVideoRepository
    {
        List<PostVideo> GetAllVideos(string? stringSearch);
        List<PostVideo> GetAllVideosPerChannel(int? id);
        List<Like> GetLikesPerVideos(int? idLike, int? idVideo);
        Like AddLike(int idVideo, Like like);
        Like UpdateLike(int? idLike, int? idVideo, Like like);
        PostVideo GetPostVideo(int? id);
        PostVideo CreatePostVideo(PostVideo postVideo, IFormFile fileImage, IFormFile fileVideo);
        PostVideo EditVideo(int id, PostVideo postVideo, IFormFile fileImage, IFormFile fileVideo);
        bool DeleteVideo(int id);
    }
}
