namespace SocialMediaWebApp.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string errorMessage) : base(errorMessage)
        {

        }
    }
}
