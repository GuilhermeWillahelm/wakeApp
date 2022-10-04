using wakeApp.Models;
using wakeApp.Dtos;

namespace wakeApp.Repositories
{
    public interface IPostVideoRepository
    {
        List<PostVideo> GetAllVideos(string? stringSearch);
        List<PostVideoDto> GetAllVideosPerChannel(int? id);
        EvaluationDto GetLikesPerVideos(int? idVideo);
        List<CommentDto> GetCommentsPerVideos(int? idVideo);
        EvaluationDto AddLike(EvaluationDto evaluation);
        Evaluation UpdateLike(int? idLike, int? idVideo, Evaluation evaluation);
        PostVideoDto GetPostVideo(int? id);
        PostVideo CreatePostVideo(PostVideo postVideo, IFormFile fileImage, IFormFile fileVideo);
        PostVideo EditVideo(int id, PostVideo postVideo, IFormFile fileImage, IFormFile fileVideo);
        bool DeleteVideo(int id);
    }
}
