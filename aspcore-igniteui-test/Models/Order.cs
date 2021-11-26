using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IgniteUI.AspCore.Test.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public DateTime? OrderDate { get; set; }
        public string ShipAddress { get; set; }
        public string ContactName { get; set; }
    }
}
