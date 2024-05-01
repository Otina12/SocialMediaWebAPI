namespace SocialMedia.Application.Dtos.DtosForGet
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public string MemberId { get; set; }
        public string Content { get; set; }
        public DateTime PostTime { get; set; }
        public DateTime? EditTime { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
    }
}
