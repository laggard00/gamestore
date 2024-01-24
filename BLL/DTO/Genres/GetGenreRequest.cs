using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO.Genres
{
    public class GetGenreRequest
    {
        public Guid id { get; set; }
        public string name { get; set; }
    }
}
