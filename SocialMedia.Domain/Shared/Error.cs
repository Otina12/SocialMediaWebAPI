namespace SocialMedia.Domain.Shared
{
    public class Error : IEquatable<Error>
    {
        public string Code { get; }
        public string Message { get; }


        public static readonly Error None = new Error("", "");
        public static readonly Error Null = new Error("Error.NullValue", "The result is null.");

        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public virtual bool Equals(Error? error)
        {
            if (error is null)
            {
                return false;
            }

            return Code == error.Code && Message == error.Message;
        }

        public override bool Equals(object? obj) => obj is Error error && Equals(error);

        public static bool operator ==(Error? a, Error? b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Error? a, Error? b) => !(a == b);



        public string ErrorMessage() => Message;
        public override string ToString() => Code;
        public override int GetHashCode() => HashCode.Combine(Code, Message);
    }
}
