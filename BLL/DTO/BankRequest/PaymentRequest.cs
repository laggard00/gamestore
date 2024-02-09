using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO.BankRequest
{
    public class PaymentRequest
    {
        public string method { get; set; }
        public Model? model { get; set; }
    }
    public class Model
    {
        public string holder { get; set; }
        public string cardNumber { get; set; }
        public int monthExpire { get; set; }
        public int yearExpire { get; set; }
        public int cvv2 { get; set; }
    }
}


