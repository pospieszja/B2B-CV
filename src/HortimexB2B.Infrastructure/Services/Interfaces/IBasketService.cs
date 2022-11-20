using HortimexB2B.Core.Domain.ShoppingCart;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HortimexB2B.Infrastructure.Services.Interfaces
{
    public interface IBasketService
    {
        Task<Basket> GetAsync(Guid userId);
        Task AddItemAsync(Guid userId, int companyId, string productId, string productName, double unitPrice, string unit, int quantity, int stockId);
        Task<Basket> CreateAsync(Guid userId, int companyId);
        Task DeleteItemAsync(Guid userId, Guid id);
        Task SetQuantitiesAsync(Guid basketId, Dictionary<string, int> items);
        Task DeleteBasketAsync(Guid guid, Guid basketId);
    }
}