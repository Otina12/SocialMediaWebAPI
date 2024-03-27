namespace SocialMediaWebApp.Helpers
{
    public class QueryObject
    {
        public bool NewestFirst { get; set; } = true;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
