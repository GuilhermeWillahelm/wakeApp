using System.ComponentModel.DataAnnotations;

namespace wakeApp.Models
{
    public class Video
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [DataType(DataType.Date)]
        public DateTime? Created { get; set; }
        public string VideoFile { get; set; } = string.Empty;
        public string ThumbImage { get; set; } = string.Empty;
        public int UserId { get; set; }
        public virtual User? User { get; set; }

    }
}
