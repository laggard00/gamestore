using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO
{
    public class OrderDTO
    {
        public Guid id { get; set; }
        public Guid customerId { get; set;}

        public DateTime date { get; set; }
    
    }
}
