using HortimexB2B.Core.Domain;
using System.Collections.Generic;

namespace HortimexB2B.Infrastructure.Repositories.Interfaces
{
    public interface ICatalogRepository
    {
        IEnumerable<CatalogItem> GetAllItems(string productName, int pageIndex, int pageSize, int companyId);
        CatalogItem GetItem(string itemId, int userId);
        int GetTotalItems(string productName, int companyId);
        IEnumerable<ItemStock> GetItemStock(string itemId);
        int GetItemUnitConverter(string itemId, string unit);
        double GetItemPrice(string itemId, int userId);
        double GetItemVat(string itemId);
        string GetItemUnitName(string unitCode);
        IEnumerable<int> GetAvaiableItemsForCustomer(int companyCode);
    }
}
