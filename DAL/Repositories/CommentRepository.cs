using GameStore.DAL.Models;
using GameStore.DAL.Repositories.RepositoryInterfaces;
using GameStore_DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly GameStoreDbContext context;

        public CommentRepository(GameStoreDbContext _context)
        {
            context = _context;
        }

        public async Task AddAsync(Comment comment)
        {
            await context.AddAsync(comment);
        }

        public void DeleteComment(Guid id, Guid commentId)
        {
            var comment = context.Comments.SingleOrDefault(x => x.GameId == id && x.Id == commentId);
            context.Remove(comment);
        }

        public async Task<IEnumerable<Comment>> GetAllByGameId(Guid key)
        {
            var comments = await context.Comments.Where(x => x.GameId == key && x.ParentCommentId == null).ToListAsync();

            foreach (var comment in comments)
            {
                await LoadChildren(comment);
            }

            return comments;
        }

        private async Task LoadChildren(Comment comment)
        {
            comment.Children = await context.Comments.Where(x => x.ParentCommentId == comment.Id).ToListAsync();

            foreach (var child in comment.Children)
            {
                await LoadChildren(child);
            }
        }

        public async Task<Comment> GetById(Guid? Id)
        {
            return context.Comments.Find(Id);
        }
    }
}
