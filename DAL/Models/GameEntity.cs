using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GameStore_DAL.Models
{
    public class GameEntity : BaseEntity
    {

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        [MaxLength(100)]
        public string GameAlias {  get; set; }

       // public Genre? Genre{ get; set; }

      //  public ICollection<Platform>? Platforms { get; set; }

        private static string ReplaceSpacesWithDashes(string input)
        {
            return input.Trim().Replace(" ", "-").ToLower();
        }
       // public GameEntity(int _id, string _name, string _desc, string gameAlias)
       // {
       //     this.Id = _id;
       //     this.Name = _name;
       //     this.Description = _desc;
       //     if (string.IsNullOrEmpty(gameAlias)) { this.GameAlias = ReplaceSpacesWithDashes(_name); }
       //     else { this.GameAlias = gameAlias; }
       //
       // }

    }

       
}
