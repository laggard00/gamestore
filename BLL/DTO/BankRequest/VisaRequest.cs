using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO.BankRequest
{
    public class VisaRequest
    {
        public double transactionAmount { get; set; }
        public string cardHolderName { get; set; }
        public string cardNumber { get; set; }
        public int expirationMonth { get; set; }
        public int cvv { get; set; }
        public long expirationYear { get; set; }
    }
}
