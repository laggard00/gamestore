using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace GameStore_DAL.Models
{
    public class PlatformEntity : BaseEntity

    {
        
        public PlatformType Type { get { return (PlatformType)Id; }  set { Id = (int)value; } }
        public ICollection<GamePlatform> GamePlatforms { get; set; }

        

    }
    public enum PlatformType
    {
        Mobile,
        Browser,
        Desktop,
        Console
    }
}