using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class PublisherBasicDTO
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "Company Name is required property")]
        public string CompanyName { get; set; }
        
       

        
    }
}
