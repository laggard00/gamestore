using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class PublisherDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Company Name is required property")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Description is required property")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Home page is required property")]
        public string HomePage { get; set; }

        public List<GameDTO>? Games { get; set; }

        
        
        


    }
}
