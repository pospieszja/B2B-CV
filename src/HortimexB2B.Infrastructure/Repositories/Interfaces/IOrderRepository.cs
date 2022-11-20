using HortimexB2B.Core.Domain.Order;
using HortimexB2B.Core.Domain.ShoppingCart;
using HortimexB2B.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HortimexB2B.Infrastructure.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrdersByCompany(int companyId, DateTime start, DateTime end);
        Task<string> CreateOrder(int salesAreaId, int companyId, int shippingAddressId, string referenceNumber, string comments, DateTime desiredDeliveryDate, Basket basket);
        Task<InvoiceFileViewModel> GetInvoiceByOrder(int orderId, int orderYear, int companyId);
        Task<BlobFileViewModel> GetFile(int documentId);
        IEnumerable<Lot> GetLotsOfOrderItem(int pos, int orderId, int orderYear);
    }
}
