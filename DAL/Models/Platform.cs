using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace GameStore_DAL.Models
{
    public class Platform : BaseEntity 

    {
        [Required]
        public string PlatformName { get; set; }


        public ICollection<GamePlatform> GamePlatforms { get; set; }



    }
}