using GameStore_DAL.Data;
using GameStore_DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO.Games {
    public class AddGameRequest
    {
        public GameDTO Game { get; set; }
        public List<Guid> Genres { get; set; }
        public List<Guid> Platforms { get; set; }
        public Guid Publisher { get; set; }
    }

    public class GameDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Key { get; set; }
        public double price { get; set; }
        public int unitInStock { get; set; }
        public int discount { get; set; }
    }

}
