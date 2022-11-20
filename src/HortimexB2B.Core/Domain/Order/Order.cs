using System;
using System.Collections.Generic;
using System.Text;

namespace HortimexB2B.Core.Domain.Order
{
    public class Order
    {
        public int OrderId { get; set; }
        public int Year { get; set; }
        public int CompanyId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int Status { get; set; }
        public bool IsInvoiceIssued { get; set; }
        public string TrackingNumber { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
