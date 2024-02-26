using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO.UserViews {
    public class UpdateUserDTO {

        public UserDTO user { get; set; }
        public IEnumerable<string> roles { get; set; }
        public string password { get; set; }
    }
}
