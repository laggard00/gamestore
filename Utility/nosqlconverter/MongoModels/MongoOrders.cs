using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace nosqlconverter.MongoModels
{
    public class MongoOrders : MongoBase
    {

        [BsonElement("OrderID")]
        public int OrderId { get; set; }

        [BsonElement("CustomerID")]
        public string CustomerId { get; set; }

        [BsonElement("EmployeeID")]
        public int EmployeeId { get; set; }

        [BsonElement("OrderDate")]
        public string OrderDate { get; set; }

        [BsonElement("RequiredDate")]
        public string RequiredDate { get; set; }

        [BsonElement("ShippedDate")]
        public string ShippedDate { get; set; }

        [BsonElement("ShipVia")]
        public int ShipVia { get; set; }

        [BsonElement("Freight")]
        public decimal Freight { get; set; }

        [BsonElement("ShipName")]
        public string ShipName { get; set; }

        [BsonElement("ShipAddress")]
        public string ShipAddress { get; set; }

        [BsonElement("ShipCity")]
        public string ShipCity { get; set; }

        [BsonElement("ShipRegion")]
        public string ShipRegion { get; set; }

        [BsonElement("ShipPostalCode")]
        public string ShipPostalCode { get; set; }

        [BsonElement("ShipCountry")]
        public string ShipCountry { get; set; }
    }
}
