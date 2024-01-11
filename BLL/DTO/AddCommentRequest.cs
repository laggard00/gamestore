using FluentAssertions.Primitives;
using FluentValidation;
using GameStore.DAL.Repositories.RepositoryInterfaces;
using GameStore_DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO
{
    public class AddCommentRequest
    {
        public CommentDTO comment { get; set; }
        public Guid? parentId { get; set; }
        public string? action { get; set; }
    }

    public class CommentDTO 
    {
        public string name { get; set; }
        public string body { get; set; }
    }

    public class AddCommentRequestValidator : AbstractValidator<AddCommentRequest>
    {
        private readonly ICommentRepository commentRepo;
        public AddCommentRequestValidator(ICommentRepository _commentRepo)
        {
            commentRepo = _commentRepo;
            RuleFor(x => x.parentId)
                                     .MustAsync(async (parentId,token) =>
                                     {
                                         return await commentRepo.ParentExist(parentId);
                                     })
                                    .When(x => x.parentId.HasValue)
                                    .WithMessage("ParentId should exist in database");
            RuleFor(x => x.action)
                                    .Must(action => action == "quote" || action == "reply")
                                    .When(x => !string.IsNullOrEmpty(x.action))
                                    .WithMessage("Action must be either 'quote' or 'reply'");

            RuleFor(x => x.comment.name)
                                        .MinimumLength(2).NotEmpty().WithMessage("Comment name is empty");
            RuleFor(x => x.comment.body)
                                        .NotEmpty().WithMessage("Comment body can't be empty");

        }

       

    }
}
