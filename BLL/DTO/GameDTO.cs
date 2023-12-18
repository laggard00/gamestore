using GameStore_DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class GameDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Description { get; set; }

        public string? GameAlias { get; set; }

        [Required(ErrorMessage = "GenreId is required.")]

        public List<GenreDTO> Genres { get; set; }
        public List<PlatformDTO> Platforms { get; set; }

        public PublisherBasicDTO Publisher { get; set; }
       //  public List<int> GenreId { get; set; }
       // 
       // 
       //  public List<string> GenreNames { get; set; }
       //
       //
       // public List<int> PlatformId { get; set; }
       // public List<string> PlatformNames { get; set; }

        
        public decimal Price { get; set; }

        

        public byte Discount { get; set; }

        public short UnitInStock { get; set; }
    }

    
}
