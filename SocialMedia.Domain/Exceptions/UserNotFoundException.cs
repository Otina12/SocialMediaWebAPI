namespace SocialMedia.Domain.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string errorMessage) : base(errorMessage)
        {

        }
    }
}
