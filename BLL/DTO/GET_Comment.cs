using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO
{
    public class GET_Comment
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string body { get; set; }
        public List<GET_Comment> childComments { get; set; }
    }
}
