using System.ComponentModel.DataAnnotations;
using wakeApp.Models;

namespace wakeApp.Dtos
{
    public class PostVideoDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [DataType(DataType.Date)]
        public DateTime? Posted { get; set; }
        public string VideoFile { get; set; } = string.Empty;
        public string ThumbImage { get; set; } = string.Empty;
        public int UserId { get; set; }
        public virtual UserDto? UserDto { get; set; }
        public int ChannelId { get; set; }
        public virtual ChannelDto? ChannelDto { get; set; }
        public int LikeId { get; set; }
        public virtual LikeDto? LikeDto { get; set; }
        public int CommentId { get; set; }
        public virtual CommentDto? CommentDto { get; set; }
    }
}
