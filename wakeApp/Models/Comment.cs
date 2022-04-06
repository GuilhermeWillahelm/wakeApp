using System.ComponentModel.DataAnnotations.Schema;

namespace wakeApp.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(2000)")]
        public string CommentText { get; set; } = string.Empty;
        public int UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
