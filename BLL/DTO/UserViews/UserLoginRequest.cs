using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO.UserViews {
    public class UserLoginRequest {

        public UserLoginModel model { get; set; }
    }

    public class UserLoginModel {
        
        public string login { get; set; }
        public string password { get; set; }
        public bool internalAuth { get; set; } = true;
    }
}
