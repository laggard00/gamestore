using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class GenreEntity : BaseEntity
    {

        [Required]
        public string Name { get; set; }

        public Guid? ParentGenreId { get; set; }

        
        
    }
}