using HortimexB2B.Core.Domain.ShoppingCart;
using HortimexB2B.Infrastructure.Repositories.Interfaces;
using HortimexB2B.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HortimexB2B.Infrastructure.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly ICatalogRepository _catalogRepository;
        private readonly IUserService _userService;

        public BasketService(IBasketRepository basketRepository, ICatalogRepository catalogRepository, IUserService userService)
        {
            _basketRepository = basketRepository;
            _catalogRepository = catalogRepository;
            _userService = userService;
        }

        public async Task AddItemAsync(Guid userId, int companyId, string productId, string productName, double unitPrice, string unit, int quantity, int stockId)
        {
            var basket = await _basketRepository.GetByUserAsync(userId);
            if (basket == null)
            {
                basket = await CreateAsync(userId, companyId);
            }

            var convertedQuantity = _catalogRepository.GetItemUnitConverter(productId, unit) * quantity;
            basket.AddBasketItem(userId, productId, productName, unitPrice, unit, quantity, convertedQuantity, stockId);

            await _basketRepository.UpdateAsync(basket);
        }

        public async Task<Basket> CreateAsync(Guid userId, int companyId)
        {
            var basket = new Basket(userId, companyId);
            await _basketRepository.AddAsync(basket);
            return basket;
        }

        public async Task DeleteBasketAsync(Guid userId, Guid basketId)
        {
            if (userId == null || basketId == null)
            {
                throw new Exception($"UserId or BasketId cannot be null.");
            }

            var basket = await _basketRepository.GetAsync(basketId);
            if (basket.UserId != userId)
            {
                throw new Exception($"User with {userId} ID is not an owner of basket with {basketId} ID.");
            }
            await _basketRepository.RemoveAsync(basket);

        }

        public async Task DeleteItemAsync(Guid userId, Guid id)
        {
            if (userId == null || id == null)
            {
                throw new Exception($"UserId and ItemId cannot be null.");
            }

            var basket = await _basketRepository.GetByUserAsync(userId);
            if (basket.UserId != userId)
            {
                throw new Exception($"User with {userId} ID is not an owner of basket with {basket.BasketId} ID.");
            }

            basket.RemoveBasketItem(id);
            await _basketRepository.UpdateAsync(basket);

            if (!basket.BasketItems.Any())
            {
                await _basketRepository.RemoveAsync(basket);
            }
        }

        public async Task<Basket> GetAsync(Guid userId)
        {
            if (userId == null)
            {
                throw new Exception($"UserId cannot be null.");
            }
            var basket = await _basketRepository.GetByUserAsync(userId);
            var user = await _userService.GetUser(userId.ToString());

            if (basket != null)
            {
                foreach (var item in basket.BasketItems)
                {
                    var convertedQuantity = _catalogRepository.GetItemUnitConverter(item.ProductId, item.Unit) * item.Quantity;
                    item.SetUnitPrice(_catalogRepository.GetItemPrice(item.ProductId, user.GraffitiId));
                    item.SetVat(_catalogRepository.GetItemVat(item.ProductId));
                    item.SetConvertedQuantity(convertedQuantity);
                    item.SetUnitName(_catalogRepository.GetItemUnitName(item.Unit));
                }
            }
            return basket;
        }

        public async Task SetQuantitiesAsync(Guid basketId, Dictionary<string, int> items)
        {
            var basket = await _basketRepository.GetAsync(basketId);
            basket.UpdateQuantities(items);
            await _basketRepository.UpdateAsync(basket);
        }
    }
}
