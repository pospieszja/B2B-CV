using System;

namespace HortimexB2B.Web.ViewModels.Basket
{
    public class BasketItemViewModel
    {
        public Guid ItemId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public double UnitPrice { get; set; }
        public double Vat { get; set; }
        public string Unit { get; set; }
        public string UnitName { get; set; }
        public int Quantity { get; set; }
        public int ConvertedQuantity { get; set; }
        public double Value
        {
            get
            {
                return UnitPrice * ConvertedQuantity;
            }
        }
        public double ValueWithVat
        {
            get
            {
                return Math.Round((UnitPrice * ConvertedQuantity) * (1 + Vat / 100), 2);
            }
        }
    }
}
