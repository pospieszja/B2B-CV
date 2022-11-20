using Dapper;
using HortimexB2B.Core.Domain.Order;
using HortimexB2B.Core.Domain.ShoppingCart;
using HortimexB2B.Infrastructure.Repositories.Interfaces;
using HortimexB2B.Infrastructure.ViewModels;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HortimexB2B.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IConfiguration _config;

        public OrderRepository(IConfiguration config)
        {
            _config = config;

            //Need for Encoding option in PostgreSQL connection string.
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        internal IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(_config.GetConnectionString("GraffitiConnection"));
            }
        }

        public async Task<string> CreateOrder(int salesAreaId, int companyId, int shippingAddressId, string referenceNumber, string comments, DateTime desiredDeliveryDate, Basket basket)
        {
            var sqlHeader = "SELECT * FROM b2b.create_customer_order(@SalesAreaId, @CompanyId, @ShippingAddressId, @ReferenceNumber, @Comments, @DesiredDeliveryDate )";
            var sqlPosition = "SELECT * FROM b2b.create_customer_order_pos(@OrderYear, @OrderId, @PositionNo, @ProductId, @ConvertedQuantity, @UnitPrice, @StockId)";
            string orderNumber;

            using (IDbConnection db = Connection)
            {
                db.Open();
                var trans = db.BeginTransaction();

                var createdOrder = await db.QuerySingleOrDefaultAsync<dynamic>(sqlHeader, new { salesAreaId, companyId, shippingAddressId, referenceNumber, comments, desiredDeliveryDate });

                var positionNo = 1;
                foreach (var basketItem in basket.BasketItems)
                {
                    await db.ExecuteAsync(sqlPosition, new { createdOrder.orderyear, createdOrder.orderid, positionNo, basketItem.ProductId, basketItem.ConvertedQuantity, basketItem.UnitPrice, basketItem.StockId });
                    positionNo++;
                }

                trans.Commit();
                orderNumber = String.Concat(createdOrder.orderid, "/", createdOrder.orderyear);
            }

            return orderNumber;
        }

        public async Task<IEnumerable<Order>> GetOrdersByCompany(int companyId, DateTime start, DateTime end)
        {
            var sql = "SELECT * FROM b2b.get_orders(@CompanyId, @Start, @End);SELECT * FROM b2b.get_order_items(@CompanyId, @Start, @End);";

            using (IDbConnection db = Connection)
            {
                db.Open();

                using (var multi = await db.QueryMultipleAsync(sql, new { @CompanyId = companyId, @Start = start, @End = end }))
                {
                    var orders = multi.Read<Order>();
                    var items = multi.Read<OrderItem>();

                    var result = orders.Select(o => { o.OrderItems.AddRange(items.Where(i => i.Year == o.Year && i.OrderId == o.OrderId)); return o; });
                    return result;
                }
            }
        }

        public IEnumerable<Lot> GetLotsOfOrderItem(int pos, int orderId, int orderYear)
        {
            var sql = "SELECT * FROM b2b.get_order_item_lots(@Pos, @OrderId, @OrderYear);";

            using (IDbConnection db = Connection)
            {
                db.Open();
                return db.Query<Lot>(sql, new { @Pos = pos, @OrderId = orderId, @OrderYear = orderYear });
            }
        }

        public async Task<InvoiceFileViewModel> GetInvoiceByOrder(int orderId, int orderYear, int companyId)
        {
            var sql = "SELECT * FROM b2b.get_invoice_document(@OrderId, @OrderYear, @CompanyId);";

            using (IDbConnection db = Connection)
            {
                db.Open();
                return await db.QuerySingleOrDefaultAsync<InvoiceFileViewModel>(sql, new { @OrderId = orderId, @OrderYear = orderYear, @CompanyId = companyId });
            }
        }

        public async Task<BlobFileViewModel> GetFile(int documentId)
        {
            var sql = "SELECT * FROM b2b.get_file(@DocumentId);";

            using (IDbConnection db = Connection)
            {
                db.Open();
                return await db.QuerySingleOrDefaultAsync<BlobFileViewModel>(sql, new { @DocumentId = documentId });
            }
        }
    }
}