using GameStore_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO {
    public class GetAllGames {

        public IEnumerable<Game> games { get; set; }
        public int totalPages { get; set; }

        public int currentPage { get; set; }
    }
}
