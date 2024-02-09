using GameStore_DAL.Data;
using GameStore_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO.Genres {
    public class UpdateGenreRequest
    {
        public GenreEntity genre { get; set; }
    }
}
