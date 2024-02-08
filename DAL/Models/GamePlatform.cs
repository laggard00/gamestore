using GameStore_DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models {
    public class GamePlatform {
        public Guid GameId { get; set; }
        public Guid PlatformId { get; set; }
    }
}
