using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO.UserViews {
    public class UserAccessRequest {
        public string targetPage { get; set; }
        public string? targetId { get; set; }
    }
}
