using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO.Comments
{
    public class GetCommentRequest
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string body { get; set; }
        public List<GetCommentRequest> childComments { get; set; }
    }
}
