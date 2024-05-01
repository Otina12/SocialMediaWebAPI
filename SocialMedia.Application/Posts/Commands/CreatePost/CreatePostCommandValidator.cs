using FluentValidation;
using SocialMedia.Domain.Interfaces;

namespace SocialMedia.Application.Posts.Commands.CreatePost
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
