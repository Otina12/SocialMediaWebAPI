namespace SocialMedia.Domain.Shared;

public sealed class ValidationResult<TValue> : Result<TValue>, IValidationResult
{
    public Error[] Errors { get; }

    public ValidationResult(Error[] errors) : base(default, false, IValidationResult.ValidationError)
    {
        Errors = errors;
    }

    public static ValidationResult<TValue> WithErrors(Error[] errors)
    {
        return new ValidationResult<TValue>(errors);
    }
}
