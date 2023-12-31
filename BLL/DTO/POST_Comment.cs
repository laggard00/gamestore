using FluentAssertions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO
{
    public class POST_Comment
    {
        public CommentDTO comment { get; set; }
        public Guid parentId { get; set; }
        public string action { get; set; }
    }

    public class CommentDTO 
    {
        public string name { get; set; }
        public string body { get; set; }
    }
}
