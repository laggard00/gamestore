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
        public int GameId { get; set; }
        public GameEntity Games { get; set; }

        public int GenreId { get; set; }
        public GenreEntity Genre { get; set; }
    }
}
