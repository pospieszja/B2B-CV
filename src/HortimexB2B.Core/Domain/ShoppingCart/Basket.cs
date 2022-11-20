using System;
using System.Collections.Generic;
using System.Linq;

namespace HortimexB2B.Core.Domain.ShoppingCart
{
    public class Basket
    {
        public Guid BasketId { get; protected set; } = Guid.NewGuid();
        public Guid UserId { get; protected set; }
        public int CompanyId { get; protected set; }
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; protected set; }
        public List<BasketItem> BasketItems => _basketItems;

        private List<BasketItem> _basketItems = new List<BasketItem>();

        protected Basket()
        {

        }

        public Basket(Guid userId, int companyId)
        {
            UserId = userId;
            CompanyId = companyId;
        }

        public void AddBasketItem(Guid userId, string productId, string productName, double unitPrice, string unit, int quantity, int convertedQuantity, int stockId)
        {
            if (!BasketItems.Any(i => i.ProductId == productId && i.Unit == unit && i.StockId == stockId))
            {
                var basketItem = new BasketItem(userId, productId, productName, unitPrice, unit, quantity, convertedQuantity, stockId);
                _basketItems.Add(basketItem);
                UpdatedAt = DateTime.UtcNow;
                return;
            }

            var existingItem = BasketItems.FirstOrDefault(i => i.ProductId == productId);
            existingItem.UpdateQuantity(quantity + existingItem.Quantity);
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveBasketItem(Guid itemId)
        {
            var item = _basketItems.SingleOrDefault(i => i.ItemId == itemId);
            if (item != null)
            {
                _basketItems.Remove(item);
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public void UpdateQuantities(Dictionary<string, int> quantities)
        {
            foreach (var item in _basketItems)
            {
                if (quantities.TryGetValue(item.ItemId.ToString(), out var quantity))
                {
                    item.UpdateQuantity(quantity);
                }
            }
        }
    }
}
