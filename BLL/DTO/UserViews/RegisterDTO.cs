using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO.UserViews {
    public class RegisterDTO {

        public UserDTO user { get; set; }
        public ICollection<string> roles { get; set; }
        public string password { get; set; }
    }

    public class UserDTO {
        public string name { get; set; }
        public string? id { get; set; }
    }
}
