using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GameStore_DAL.Models
{
    public class Game:BaseEntity
    {
        
         [Required]
         [MaxLength(250)]
         public string Name { get; set; }

         [MaxLength(500)]
         public string? Description { get; set; }

         private string _gameAlias;

         [Required]
         [MaxLength(100)]
         public string Key
         {
             get
             {
                 if (string.IsNullOrEmpty(_gameAlias))
                 {
                     _gameAlias = Name.Replace(" ", "-").ToLower();
                 }
        
                 return _gameAlias;
             }
             set => _gameAlias = value;
         }
        
        [Column(TypeName = "decimal(18,4)")]
        public double Price { get; set; }

        public int Discount { get; set; }

        public int UnitInStock { get; set; }

        public Guid PublisherId { get; set; } 
       

     

    }

       
}
