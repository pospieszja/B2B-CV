using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HortimexB2B.Core.Domain.Order;
using HortimexB2B.Core.Domain.ShoppingCart;
using HortimexB2B.Infrastructure.Identity;
using HortimexB2B.Infrastructure.Repositories.Interfaces;
using HortimexB2B.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace HortimexB2B.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly INotificationService _notificationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContext;

        public OrderService(IOrderRepository orderRepository, ICompanyRepository companyRepository, INotificationService notificationService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContext )
        {
            _orderRepository = orderRepository;
            _companyRepository = companyRepository;
            _notificationService = notificationService;
            _userManager = userManager;
            _httpContext = httpContext;
        }

        public async Task CreateOrdersAsync(int salesAreaId, int companyId, int shippingAddressId, string referenceNumber, string comments, DateTime desiredDeliveryDate, Basket basket)
        {
            var orderNumber = await _orderRepository.CreateOrder(salesAreaId, companyId, shippingAddressId, referenceNumber, comments, desiredDeliveryDate, basket);

            if (!String.IsNullOrEmpty(orderNumber))
            {
                var user = await _userManager.GetUserAsync(_httpContext.HttpContext.User);

                var tags = new Dictionary<string, string>();
                tags.Add("order_number", orderNumber);

                await _notificationService.BroadcastAsync(user, tags, "ORDER_CREATED", "PL");
            }            
        }

        public IEnumerable<ShippingAddress> GetAllShippingAddress(int companyId)
        {
            return _companyRepository.GetAllShippingAddress(companyId);
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(int companyId, DateTime start, DateTime end)
        {
            var orders = await _orderRepository.GetOrdersByCompany(companyId, start, end);

            return orders.Select(o => { o.OrderItems.ForEach(i => i.Lots.AddRange((_orderRepository.GetLotsOfOrderItem(i.PositionNumber, i.OrderId, i.Year)))); return o; });
        }
    }
}
