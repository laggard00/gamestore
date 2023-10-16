using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace GameStore_DAL.Models
{
    public class GenreEntity:BaseEntity
    {
        
        [Required]
        public string GenreName { get; set; }

        
        
    }
}