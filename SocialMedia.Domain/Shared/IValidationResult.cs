
namespace SocialMedia.Domain.Shared;

public interface IValidationResult
{
    public static readonly Error ValidationError = new Error("ValidationError", "Validation problem occured");

    Error[] Errors { get; }
}
