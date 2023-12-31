using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace GameStore_DAL.Models
{
    public class GenreEntity : BaseEntity
    {

        [Required]
        public string Name { get; set; }

        public Guid? ParentGenreId { get; set; }

    }
}