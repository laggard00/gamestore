using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO
{
    public class POST_GenreDTO
    {
        public GenreDTO genre { get; set; }
    }
    public class GenreDTO { 
        public string name { get; set; }
        public Guid parentGenreId { get; set; }
    }
}
