namespace wakeApp.Dtos
{
    public class ViewModel
    {
        public int CountLike { get; set; }
        public int CountDislike { get; set; }
        public int CountFollowers { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public ChannelDto? ChannelDto { get; set; }
        public PostVideoDto? PostVideoDto { get; set; }
        public List<PostVideoDto>? PostVideoDtos { get; set; }
        public EvaluationDto? EvaluationDto { get; set; }
        public CommentDto? CommentDto { get; set; }
        public List<CommentDto>? CommentDtos { get; set; }
    }
}
