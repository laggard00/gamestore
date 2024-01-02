using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.DAL.Models;
using GameStore_DAL.Data;
using GameStore_DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.Services
{
    public class CommentService
    {
        private IUnitOfWork uow { get; set; }
        private IMapper mapper { get; set; }
        public CommentService(IUnitOfWork uow, IMapper map)
        {
            this.uow = uow;
            mapper = map;
        }

        public async Task FormatCommentAndAdd(string key, POST_Comment commentRequest)
        {
            var parentComment =await uow.CommentRepository.GetById(commentRequest.parentId);
            string commentBody=commentRequest.comment.body;
            var game = await uow.GamesRepository.GetGameByAlias(key);


            if (parentComment is not null)
            {
                switch (commentRequest.action.ToLower())
                {
                    case "quote": commentBody = $"\"{parentComment.Body}\" {commentRequest.comment.body}"; break;
                    case "reply": commentBody = $"\"{parentComment.Name}\" {commentRequest.comment.body}"; break;
                    default: commentBody = commentRequest.comment.body; break;
                }

            }
            
            await uow.CommentRepository.AddAsync(new Comment { GameId = game.Id, Body = commentBody, Name = commentRequest.comment.name, ParentCommentId = commentRequest.parentId }); 
           
            try
            {
                await uow.SaveAsync();
            }
            catch(Exception ex) { var b = ex.Message; return; }
        }

        public async Task<IEnumerable<GET_Comment>> GetAllComentsWithIndefiniteChildren(string key)
        {
            var game = await uow.GamesRepository.GetGameByAlias(key);
            List<GET_Comment> comments = new List<GET_Comment>();
            var allIndefiniteComments = await uow.CommentRepository.GetAllByGameId(game.Id);
            return allIndefiniteComments.Select(x => mapper.Map<GET_Comment>(x));
        }

        public async Task DeleteCommentByGameKeyAndCommentId(string key, Guid commentId)
        {
            var game = await uow.GamesRepository.GetGameByAlias(key);
            uow.CommentRepository.DeleteComment(game.Id, commentId);
            await uow.SaveAsync();
        }
    }
}
