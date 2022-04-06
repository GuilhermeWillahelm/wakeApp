namespace wakeApp.Models
{
    public class Like
    {
        public int Id { get; set; }
        public int CountLike { get; set; }
        public int CountDislike { get; set; }
        public int UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
