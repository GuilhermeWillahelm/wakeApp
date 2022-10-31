using wakeApp.Models;
using wakeApp.Dtos;

namespace wakeApp.Repositories
{
    public interface IPostVideoRepository
    {
        List<PostVideoDto> GetAllVideos(string? stringSearch);
        List<PostVideoDto> GetAllVideosPerChannel(int? id);
        int GetLikesPerVideos(int? idVideo);
        List<CommentDto> GetCommentsPerVideos(int? idVideo);
        int GetFollowersPerChannel(int? idChannel);
        EvaluationDto AddLike(EvaluationDto evaluation);
        CommentDto AddComment(CommentDto comment);
        Evaluation UpdateLike(int? idLike, int? idVideo, Evaluation evaluation);
        PostVideoDto GetPostVideo(int? id);
        PostVideo CreatePostVideo(PostVideo postVideo, IFormFile fileImage, IFormFile fileVideo);
        PostVideo EditVideo(int id, PostVideo postVideo, IFormFile fileImage, IFormFile fileVideo);
        bool DeleteVideo(int id);
    }
}
