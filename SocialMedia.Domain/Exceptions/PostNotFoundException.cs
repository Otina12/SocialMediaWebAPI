namespace SocialMedia.Domain.Exceptions
{
    public class PostNotFoundException : Exception
    {
        public PostNotFoundException(string errorMessage) : base(errorMessage)
        {

        }
    }
}
