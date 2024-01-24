using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO.OrderGame
{
    public class OrderGameDTO
    {
        public Guid productId { get; set; }
        public double price { get; set; }
        public int quantity { get; set; }
        public int discount { get; set; }
    }
}
