using System.ComponentModel.DataAnnotations;

namespace wakeApp.Models
{
    public class Channel
    {
        public int Id { get; set; }
        public string ChannelName { get; set; } = string.Empty;
        public string ChannelDescription { get; set; } = string.Empty;
        [DataType(DataType.Date)]
        public DateTime CreatedChannel { get; set; }
        public string ImageBanner { get; set; } = string.Empty;
        public int UserId { get; set; }
        public virtual User? User { get; set; }
        public int FollwerId { get; set; }
        public virtual Follower? Follower { get; set; }

    }
}
