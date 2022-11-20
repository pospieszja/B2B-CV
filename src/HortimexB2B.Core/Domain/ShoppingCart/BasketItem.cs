using System;

namespace HortimexB2B.Core.Domain.ShoppingCart
{
    public class BasketItem
    {
        public Guid ItemId { get; protected set; } = Guid.NewGuid();
        public Guid UserId { get; protected set; }
        public string ProductId { get; protected set; }
        public string ProductName { get; protected set; }
        public double UnitPrice { get; protected set; }
        public double Vat { get; protected set; }
        public string Unit { get; protected set; }
        public string UnitName { get; protected set; }
        public int Quantity { get; protected set; }
        public int ConvertedQuantity { get; protected set; }
        public int StockId { get; protected set; }

        protected BasketItem()
        {

        }

        public BasketItem(Guid userId, string productId, string productName, double unitPrice, string unit, int quantity, int convertedQuantity, int stockId)
        {
            UserId = userId;
            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Unit = unit;
            Quantity = quantity;
            ConvertedQuantity = convertedQuantity;
            StockId = stockId;
        }

        public void SetConvertedQuantity(int quantity)
        {
            ConvertedQuantity = quantity;
        }

        public void UpdateQuantity(int quantity)
        {
            Quantity = quantity;
        }

        public void SetUnitPrice(double unitPrice)
        {
            UnitPrice = unitPrice;
        }

        public void SetVat(double vat)
        {
            Vat = vat;
        }

        public void SetUnitName(string unitName)
        {
            UnitName = unitName;
        }
    }
}
