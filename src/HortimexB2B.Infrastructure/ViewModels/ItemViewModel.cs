using HortimexB2B.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace HortimexB2B.Infrastructure.ViewModels
{
    public class ItemViewModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsDangerousGood { get; set; }
        public double UnitPrice { get; set; }
        public IEnumerable<ItemStock> Stock { get; set; }
    }
}
