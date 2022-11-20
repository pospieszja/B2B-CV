using Dapper;
using HortimexB2B.Core.Domain;
using HortimexB2B.Core.Domain.Order;
using HortimexB2B.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HortimexB2B.Infrastructure.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IConfiguration _config;

        public CompanyRepository(IConfiguration config)
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

        public CompanyAddress GetAddress(int companyId)
        {
            var sql = "select adres as street, kod_miasta as postalcode, miasto as city from g.spd_kontrahenci where id_kontrahenta = @CompanyId ;";

            using (IDbConnection db = Connection)
            {
                db.Open();
                return db.QuerySingleOrDefault<CompanyAddress>(sql, new { CompanyId = companyId });
            }
        }

        public IEnumerable<ShippingAddress> GetAllShippingAddress(int companyId)
        {
            var sql = "SELECT lp AS id, opis AS name, adres AS street, miasto AS city, kodpocztowy AS postalcode, poczta AS post FROM g.spd_kontrah_adresy WHERE id_kontrah = @CompanyId ;";

            using (IDbConnection db = Connection)
            {
                db.Open();
                return db.Query<ShippingAddress>(sql, new { CompanyId = companyId });
            }
        }

        public CompanyBilling GetBilling(int companyId)
        {
            var companyBilling = new CompanyBilling();

            var sql = "select pole_r1 FROM g.spd_kontrahenci_info_dodatkow where rodzaj_info = 16 and id_kontrahenta = @CompanyId ;";
            using (IDbConnection db = Connection)
            {
                db.Open();
                companyBilling.CreditLimit = db.QuerySingleOrDefault<double>(sql, new { CompanyId = companyId });
            }

            sql = "select pole_l1 FROM g.spd_kontrahenci_info_dodatkow where rodzaj_info = 1 and id_kontrahenta = @CompanyId ;";
            using (IDbConnection db = Connection)
            {
                db.Open();
                companyBilling.PaymentTerms = db.QuerySingleOrDefault<int>(sql, new { CompanyId = companyId });
            }

            return companyBilling;
        }

        public Company GetCompany(int companyId)
        {
            var sql = "select TRIM(skrot_nazwy) as shortname, TRIM(nazwa_kontrahenta) as name, CASE WHEN NIP <> '' THEN NIP ELSE nip_ue END as nip from g.spd_kontrahenci where id_kontrahenta = @CompanyId ;";

            using (IDbConnection db = Connection)
            {
                db.Open();
                return db.QuerySingleOrDefault<Company>(sql, new { CompanyId = companyId });
            }
        }
        public SalesManager GetSalesManagerInfo(int companyId)
        {
            var sql = "SELECT * FROM b2b.get_sales_manager(@companyId);";

            using (IDbConnection db = Connection)
            {
                db.Open();
                return db.QuerySingleOrDefault<SalesManager>(sql, new { companyId });
            }
        }
    }
}
