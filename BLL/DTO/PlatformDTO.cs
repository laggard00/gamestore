using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class PlatformDTO
    {
        [Range(1,4, ErrorMessage ="platforms of those types don't exist")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Platform Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }
    }
}
