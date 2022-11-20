using HortimexB2B.Core.Domain.ShoppingCart;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HortimexB2B.Infrastructure.Repositories.Interfaces
{
    public interface IBasketRepository
    {
        Task<Basket> GetAsync(Guid basketId);
        Task<Basket> GetByUserAsync(Guid userId);
        Task AddAsync(Basket basket);
        Task UpdateAsync(Basket basket);
        Task RemoveAsync(Basket basket);
    }
}
