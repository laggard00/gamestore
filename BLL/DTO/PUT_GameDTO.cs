using GameStore_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO
{
    public class PUT_GameDTO
    {
        public Game Game { get; set; }
        public List<Guid> Genre { get; set; }
        public List<Guid> Platform { get; set; }

       
    }
}
