using DAL.Repositories;
using GameStore_DAL.Data;
using GameStore_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO.Platform {
    public class UpdatePlatformRequest
    {
        public GameStore_DAL.Models.Platform platform { get; set; }
    }
}
