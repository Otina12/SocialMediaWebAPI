namespace SocialMedia.Domain.Shared;

public sealed class ValidationResult : Result, IValidationResult
{
    public Error[] Errors { get; set; }

    private ValidationResult(Error[] errors) : base(false, IValidationResult.ValidationError)
    {
        Errors = errors;
    }

    public static ValidationResult WithErrors(Error[] errors)
    {
        return new ValidationResult(errors);
    }
}
