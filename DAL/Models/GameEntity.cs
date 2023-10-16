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
    public class GameEntity:BaseEntity
    {
        
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

         [MaxLength(500)]
         public string Description { get; set; }

         private string _gameAlias;

         [Required]
         [MaxLength(100)]
         public string GameAlias
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


        public int GenreId { get; set; }

        
        public GenreEntity? Genre { get; set; }



        public ICollection<GamePlatform>? GamePlatforms { get; set; }
       //
       // private static string ReplaceSpacesWithDashes(string input)
       // {
       //     return input.Trim().Replace(" ", "-").ToLower();
       // }
       // public GameEntity(int id, string name, string description, string gameAlias)
       // {
       //     Id = id;
       //     Name = name;
       //     Description = description;
       //     if (string.IsNullOrEmpty(gameAlias)) { this.GameAlias = ReplaceSpacesWithDashes(name); }
       //     else { this.GameAlias = gameAlias; }
       // }

    }

       
}
