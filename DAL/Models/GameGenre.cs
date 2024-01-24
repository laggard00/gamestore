using GameStore_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class GameGenre
    {
        public Guid GameId { get; set; }

        public Guid GenreId { get; set; }
        
        
    }
}
