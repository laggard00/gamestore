
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models {
    public class Publisher : BaseEntity {

        public string CompanyName { get; set; }
        public string? Description { get; set; }
        public string? HomePage { get; set; }
    }
}
