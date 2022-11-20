using System.Collections.Generic;

namespace HortimexB2B.Core.Domain.Invoice
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public int Year { get; set; }
        public string Register { get; set; }
        public string Type { get; set; }
        public int Number { get; set; }
        public List<InvoiceItem> OrderItems { get; set; } = new List<InvoiceItem>();
    }
}
