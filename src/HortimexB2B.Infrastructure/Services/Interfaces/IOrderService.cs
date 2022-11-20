using HortimexB2B.Core.Domain.Order;
using HortimexB2B.Core.Domain.ShoppingCart;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HortimexB2B.Infrastructure.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrdersAsync(int companyId, DateTime start, DateTime end);
        IEnumerable<ShippingAddress> GetAllShippingAddress(int companyId);
        Task CreateOrdersAsync(int salesAreaId, int companyId, int shippingAddressId, string referenceNumber, string comments, DateTime desiredDeliveryDate, Basket basket);
    }
}
