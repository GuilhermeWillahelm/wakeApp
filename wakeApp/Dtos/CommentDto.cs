using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using wakeApp.Models;

namespace wakeApp.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(2000)")]
        public string CommentText { get; set; } = string.Empty;
        public int UserId { get; set; }
        public virtual User? User { get; set; }
        public int ChannelId { get; set; }
        public virtual Channel? Channel { get; set; }
        public int PostId { get; set; }
        public virtual PostVideo? PostVideo { get; set; }
        public bool Flag { get; set; }
        public List<PostVideo>? PostVideos { get; set; }
    }
}
