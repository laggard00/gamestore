using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class GameDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GameAlias { get; set;
        }
        public int GenreId { get; set; }
        public List<int> PlatformId { get; set; }
    }
}
