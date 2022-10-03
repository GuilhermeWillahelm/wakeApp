namespace wakeApp.Dtos
{
    public class ViewModel
    {
        public int CountLike { get; set; }
        public int CountDislike { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public PostVideoDto? PostVideoDto { get; set; }
        public EvaluationDto? EvaluationDto { get; set; }
        public List<CommentDto>? CommentDtos { get; set; }
    }
}
