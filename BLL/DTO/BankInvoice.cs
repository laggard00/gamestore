using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO
{
    public class BankInvoice
    {
        public Guid UserId { get; set; }
        public Guid OrderId { get; set; }
        public DateTime CreationDate { get; set; }
        public double Sum { get; set; }
    }
}
