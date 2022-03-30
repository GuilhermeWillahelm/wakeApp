namespace wakeApp.Models
{
    public class Follower
    {
        public int Id { get; set; }
        public string FollowerName { get; set; } = string.Empty;
        public int UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
