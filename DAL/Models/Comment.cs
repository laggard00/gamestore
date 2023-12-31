using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.Models
{
    public class Comment : BaseEntity
    {
        public string Name { get; set; }
        public string Body { get; set; }

        public Guid? ParentCommentId { get; set; } 
        public virtual Comment ParentComment { get; set; }

        public Guid GameId { get; set; }

        public virtual List<Comment> Children { get; set; } = new List<Comment>();  
    }
}
