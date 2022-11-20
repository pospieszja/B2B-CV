using HortimexB2B.Core.Domain;
using HortimexB2B.Core.Domain.Order;
using System.Collections.Generic;

namespace HortimexB2B.Infrastructure.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        Company GetCompany(int companyId);
        CompanyAddress GetAddress(int companyId);
        IEnumerable<ShippingAddress> GetAllShippingAddress(int companyId);
        CompanyBilling GetBilling(int companyId);
        SalesManager GetSalesManagerInfo(int companyId);
    }
}
