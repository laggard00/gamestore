using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO
{
    public class POST_Publisher
    {
        public PublisherDTO publisher { get; set; }
    }
    public class PublisherDTO
    { 
        public string companyName { get; set; }
        public string homePage { get; set; }
        public string description { get; set; }
    }
}
