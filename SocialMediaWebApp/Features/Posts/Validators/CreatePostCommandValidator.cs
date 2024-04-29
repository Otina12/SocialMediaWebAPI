using FluentValidation;
using SocialMediaWebApp.Core.IConfiguration;
using SocialMediaWebApp.Features.Posts.Commands;

namespace SocialMediaWebApp.Features.Posts.Validators
{
    public sealed class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {

        public CreatePostCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(c => c.CommunityId).MustAsync(async (id, _) =>
            {
                return await unitOfWork.Communities.CommunityExists(id);
            }).WithMessage("The community doesn't exist");


            RuleFor(c => c.PostDto.Content)
                .NotEmpty()
                .WithMessage("The post cannot be empty");
        }
    }
}
