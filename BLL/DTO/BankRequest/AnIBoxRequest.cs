using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO.BankRequest
{
    public class AnIBoxRequest
    {
        public double transactionAmount { get; set; }
        public Guid accountNumber { get; set; }
        public Guid invoiceNumber { get; set; }

    }
}
