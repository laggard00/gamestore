using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore_DAL.Models;

namespace DAL.Models
{
    public class Publisher:BaseEntity
    {
        public string CompanyName { get; set; }
        public string? Description { get; set; }
        public string? HomePage { get; set; }
        
        
    }
}
