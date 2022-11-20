using Dapper;
using HortimexB2B.Core.Domain;
using HortimexB2B.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HortimexB2B.Infrastructure.Repositories
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly IConfiguration _config;

        public CatalogRepository(IConfiguration config)
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

        public IEnumerable<CatalogItem> GetAllItems(string productName, int pageIndex, int pageSize, int companyId)
        {
            var sql = "SELECT * FROM b2b.get_items(@ProductName, @Offset, @limit, @CompanyId)";
            int offset = (pageIndex - 1) * pageSize;
            int limit = pageSize;

            using (IDbConnection db = Connection)
            {
                db.Open();
                return db.Query<CatalogItem>(sql, new { Offset = offset, Limit = limit, ProductName = productName.ToUpper(), CompanyId = companyId });
            }
        }

        public CatalogItem GetItem(string itemId, int userId)
        {
            //var sql = @"select id_materialu as id, TRIM(nazwa_indeksu) as name, TRIM(indeks) as code, b2b.""GetItemPrice""(@ItemId, @UserId) as unitprice, exists (SELECT opis FROM g.mzk_protokoly_poz where id_zrodla1 = id_materialu and promptopisu = 'Produkt niebezpieczny') as isdangerousgood from g.gm_kartoteki as K where id_magazynu = 1 and indeks = @ItemId;";
            var sql = @"SELECT * FROM b2b.get_item(@ItemId, @UserId)";

            using (IDbConnection db = Connection)
            {
                db.Open();
                return db.QueryFirstOrDefault<CatalogItem>(sql, new { ItemId = itemId, UserId = userId });
            }
        }

        public int GetTotalItems(string productName, int companyId)
        {
            var sql = "SELECT * FROM b2b.get_total_count_of_items(@ProductName, @CompanyId);";

            using (IDbConnection db = Connection)
            {
                db.Open();
                return db.ExecuteScalar<int>(sql, new { ProductName = productName.ToUpper(), CompanyId = companyId });
            }
        }

        public IEnumerable<ItemStock> GetItemStock(string itemId)
        {
            var sql = @"SELECT * FROM b2b.""GetItemStockPerUnit""(@ItemId)";

            using (IDbConnection db = Connection)
            {
                db.Open();
                return db.Query<ItemStock>(sql, new { ItemId = itemId });
            }
        }

        public int GetItemUnitConverter(string itemId, string unit)
        {
            var sql = @"SELECT * FROM b2b.""GetItemUnitConverter""(@ItemId, @Unit)";

            using (IDbConnection db = Connection)
            {
                db.Open();
                return db.ExecuteScalar<int>(sql, new { ItemId = itemId, Unit = unit });
            }
        }

        public double GetItemPrice(string itemId, int userId)
        {
            var sql = @"SELECT * FROM b2b.""GetItemPrice""(@ItemId, @UserId)";

            using (IDbConnection db = Connection)
            {
                db.Open();
                return db.ExecuteScalar<double>(sql, new { ItemId = itemId, UserId = userId });
            }
        }

        public double GetItemVat(string itemId)
        {
            var sql = @"SELECT * FROM b2b.""GetItemVat""(@ItemId)";

            using (IDbConnection db = Connection)
            {
                db.Open();
                return db.ExecuteScalar<double>(sql, new { ItemId = itemId });
            }
        }

        public string GetItemUnitName(string unitCode)
        {
            var sql = @"SELECT * FROM b2b.""GetItemUnitName""(@UnitCode)";

            using (IDbConnection db = Connection)
            {
                db.Open();
                return db.ExecuteScalar<string>(sql, new { UnitCode = unitCode });
            }
        }

        public IEnumerable<int> GetAvaiableItemsForCustomer(int companyCode)
        {
            var sql = @"SELECT * FROM b2b.get_avaiable_items_for_customer(@CompanyCode)";

            using (IDbConnection db = Connection)
            {
                db.Open();
                return db.Query<int>(sql, new { CompanyCode = companyCode });
            }
        }
    }
}
