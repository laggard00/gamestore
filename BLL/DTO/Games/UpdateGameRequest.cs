using GameStore_DAL.Data;
using GameStore_DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO.Games {
    public class UpdateGameRequest
    {
        public Game Game { get; set; }
        public List<Guid> Genres { get; set; }
        public List<Guid> Platforms { get; set; }
        public Guid Publisher { get; set; }


    }

}
