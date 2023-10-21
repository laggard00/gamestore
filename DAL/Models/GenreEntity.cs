using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace GameStore_DAL.Models
{
    public class GenreEntity : BaseEntity
    {

        [Required]
        public string GenreName { get; set; }

        public ICollection<GenreEntity>? SubGenre { get; set; }

        public int? ParentGenreId { get; set; }

        
        public ICollection<GameGenre>? GameGenres { get; set; }






    }
}