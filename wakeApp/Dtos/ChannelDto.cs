using System.ComponentModel.DataAnnotations;
using wakeApp.Models;

namespace wakeApp.Dtos
{
    public class ChannelDto
    {
        public int Id { get; set; }
        public string ChannelName { get; set; } = string.Empty;
        public string SubtitleChannel { get; set; } = string.Empty;
        public string ChannelDescription { get; set; } = string.Empty;
        [DataType(DataType.Date)]
        public DateTime CreatedChanel { get; set; }
        public string ImageBanner { get; set; } = string.Empty;
        public string IconChannel { get; set; } = string.Empty;
        public int CountFollows { get; set; }
        public int UserId { get; set; }
        public virtual UserDto? User { get; set; }
        public List<PostVideo>? PostVideos { get; set; }
    }
}
