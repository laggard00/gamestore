using GameStore_DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class GamePlatform
    {
        public int GameId { get; set; }
        public GameEntity Game { get; set; }
        
        public int PlatformId { get; set; }

        public Platform Platform { get; set; }
        
        
    }
}
