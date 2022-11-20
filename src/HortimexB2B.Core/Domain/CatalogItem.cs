using System;
using System.Collections.Generic;
using System.Text;

namespace HortimexB2B.Core.Domain
{
    public class CatalogItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public double UnitPrice { get; set; }
        public double Vat { get; set; }
        public bool IsDangerousGood { get; set; }
    }
}
