using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore_DAL.Models;

namespace DAL.Models
{
    public class PublisherEntity:BaseEntity
    {

        [Column(TypeName = "nvarchar(MAX)")]
        [MaxLength(40)]
        public string CompanyName { get; set; }

        
        public string? Descritpion { get; set; }
        
        public string HomePage { get; set; }

        public ICollection<GameEntity>? Games { get; set; }
        
        
    }
}
