using GameStore.DAL.Models;
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
    public class CommentRepository
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
            var comment = context.Comments.Where(x => x.GameId == id && x.Id == commentId).SingleOrDefault();
            context.Remove(comment);
        }

        public async Task<IEnumerable<Comment>> GetAllByGameId(Guid key)
        {
            return await context.Comments.Where(x => x.GameId == key && x.ParentCommentId == null).Include(x=> x.Children).ToListAsync();
        }

        public async Task<Comment> GetById(Guid Id)
        {
            return context.Comments.Find(Id);
        }
    }
}
