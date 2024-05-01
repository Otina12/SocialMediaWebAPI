namespace SocialMedia.Application.Dtos.DtosForGet
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string MemberId { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? EditTime { get; set; }
        public int LikeCount { get; set; }
        public Guid? IsReplyToId { get; set; }
    }
}
