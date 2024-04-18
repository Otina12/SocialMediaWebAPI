namespace SocialMediaWebApp.Exceptions
{
    public class CommunityNotFoundException : Exception
    {
        public CommunityNotFoundException(string errorMessage) : base(errorMessage)
        {

        }
    }
}
