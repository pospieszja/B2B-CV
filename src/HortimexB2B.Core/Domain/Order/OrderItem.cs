using System;
using System.Collections.Generic;
using System.Text;

namespace HortimexB2B.Core.Domain.Order
{
    public class OrderItem
    {
        public int OrderId { get; set; }
        public int Year { get; set; }
        public int PositionNumber { get; set; }
        public string SKU { get; set; }
        public string ProductName { get; set; }
        public double Quantity { get; set; }
        public double UnitPrice { get; set; }
        public string Currency { get; set; }
        public bool IsDangerousGood { get; set; }
        public List<Lot> Lots { get; set; } = new List<Lot>();
    }
}
