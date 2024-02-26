using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO.UserViews {
    public class UpdateRoleRequest {
        public RoleDTO role { get; set; }
        public IEnumerable<string> permissions { get; set; }
    }
}
