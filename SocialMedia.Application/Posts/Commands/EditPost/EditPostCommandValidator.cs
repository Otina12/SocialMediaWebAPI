using FluentValidation;
using SocialMedia.Domain.Interfaces;

namespace SocialMedia.Application.Posts.Commands.EditPost
{
    public class EditPostCommandValidator : AbstractValidator<EditPostCommand>
    {
        public EditPostCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(c => c.PostId).MustAsync(async (postId, _) =>
            {
                return await unitOfWork.Posts.PostExists(postId);
            }).WithMessage($"The post doesn't exist");

            // checking if user is the creator of a post
            RuleFor(c => new { c.PostId, c.UserId }).MustAsync(async (postAndUserIds, _) =>
            {
                var post = await unitOfWork.Posts.GetByIdAsync(postAndUserIds.PostId);
                var creatorId = post!.MemberId;

                return creatorId == postAndUserIds.UserId;
            }).WithMessage("Only creator of the post can edit it")
            .WhenAsync(async (command, _) => await unitOfWork.Posts.PostExists(command.PostId));

            RuleFor(c => c.postDto.Content)
                .MinimumLength(1).WithMessage("Post cannot be empty")
                .MaximumLength(1000).WithMessage("The post is too long");

        }
    }
}
