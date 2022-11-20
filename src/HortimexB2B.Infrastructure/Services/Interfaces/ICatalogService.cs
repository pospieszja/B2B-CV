using HortimexB2B.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HortimexB2B.Infrastructure.Services.Interfaces
{
    public interface ICatalogService
    {
        CatalogIndexViewModel GetCatalogItems(string productName, int pageIndex, int pageSize, int companyId);
        ItemViewModel GetItem(string itemId, int userId);
        bool IsEnoughQuantityOnStock(string itemId, string unit, int demandQuantity);
    }
}
