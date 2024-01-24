using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO.Platform {
    public class AddPlatformRequest
    {
        public PlatformDTO platform { get; set; }
    }
    public class PlatformDTO
    {
        public string type { get; set; }
    }
}
