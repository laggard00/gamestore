using FluentAssertions.Primitives;
using GameStore_DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO.Comments {
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
}
