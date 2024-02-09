using FluentValidation;
using GameStore.BLL.DTO.Comments;
using GameStore.DAL.Repositories.RepositoryInterfaces;

namespace GameStore.BLL.Validators
{
    public class AddCommentRequestValidator : AbstractValidator<AddCommentRequest>
    {
        private readonly ICommentRepository commentRepo;
        public AddCommentRequestValidator(ICommentRepository _commentRepo)
        {
            commentRepo = _commentRepo;
            RuleFor(x => x.parentId)
                                    .Must(commentRepo.ParentExist)
                                    .When(x => x.parentId.HasValue)
                                    .WithMessage("ParentId should exist in database");
            RuleFor(x => x.action)
                                    .Must(action => action.ToLower() == "quote" || action.ToLower() == "reply")
                                    .When(x => !string.IsNullOrEmpty(x.action))
                                    .WithMessage("Action must be either 'quote' or 'reply'");

            RuleFor(x => x.comment.name)
                                        .MinimumLength(2).NotEmpty().WithMessage("Comment name is empty");
            RuleFor(x => x.comment.body)
                                        .NotEmpty().WithMessage("Comment body can't be empty");

        }



    }
}
