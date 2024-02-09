using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace GameStore_DAL.Models {
    public class Platform : BaseEntity {
        public string Type { get; set; }
    }

}