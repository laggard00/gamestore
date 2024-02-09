using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nosqlconverter.MongoModels {
    public class MongoOrderDetails : MongoBase{

        public int OrderID { get; set; }
        public int ProductID { get;set; }
        public double UnitPrice { get; set; }
        public double Discount { get; set; }
        public int Quantity { get; set; }

        public MongoProducts Game { get; set; }

    }
}
