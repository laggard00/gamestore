using System.ComponentModel.DataAnnotations;

namespace GameStore_DAL.Models
{
    public class Genre :BaseEntity
    {
        [Required]
        public string GenreName { get; set; }

        
    }
}