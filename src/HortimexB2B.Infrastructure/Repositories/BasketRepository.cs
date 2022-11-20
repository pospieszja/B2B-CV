using HortimexB2B.Core.Domain.ShoppingCart;
using HortimexB2B.Infrastructure.EF;
using HortimexB2B.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace HortimexB2B.Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly B2BContext _context;

        public BasketRepository(B2BContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Basket basket)
        {
            await _context.Baskets.AddAsync(basket);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Basket basket)
        {
            _context.Baskets.Update(basket);
            await _context.SaveChangesAsync();
        }

        public async Task<Basket> GetAsync(Guid basketId)
        {
            var basket = await _context.Baskets.Include(b => b.BasketItems).SingleOrDefaultAsync(x => x.BasketId == basketId);
            return basket;
        }

        public async Task<Basket> GetByUserAsync(Guid userId)
        {
            var basket = await _context.Baskets.Include(b=>b.BasketItems).SingleOrDefaultAsync(x => x.UserId == userId);
            return basket;
        }

        public async Task RemoveAsync(Basket basket)
        {
            _context.Baskets.Remove(basket);
            await _context.SaveChangesAsync();
        }
    }
}
