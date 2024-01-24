using GameStore_DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO.Genres {
    public class AddGenreRequest
    {
        public GenreDTO genre { get; set; }
    }
    public class GenreDTO
    {
        public string name { get; set; }
        public Guid? parentGenreId { get; set; }
    }
}
