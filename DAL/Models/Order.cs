using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.Models {
    public class Order : BaseEntity {
        public DateTime Date { get; set; }
        public Guid CustomerId { get; set; }
        public string Status {
            get { return _status.ToString(); }
            set {
                if (Enum.TryParse(value, true, out Statuses parsedStatus)) {
                    _status = parsedStatus;
                } else {
                    throw new ArgumentException("Invalid status value");
                }
            }
        }
        private Statuses _status;
        public enum Statuses {
            Open,
            Checkout,
            Paid,
            Cancelled
        }
    }

}
