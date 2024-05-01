namespace SocialMedia.Application.Dtos.DtosForGet
{
    public class FollowerDto
    {
        public string UserName { get; set; }
        public string? Bio { get; set; }
        public string? ProfilePhotoUrl { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
