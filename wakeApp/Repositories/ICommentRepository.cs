using wakeApp.Dtos;

namespace wakeApp.Repositories
{
    public interface ICommentRepository
    {
        List<CommentDto> GetCommentsPerVideos(int? idVideo);
        CommentDto AddComment(CommentDto comment);
        bool DeleteComment(int id, int idUser);
    }
}
