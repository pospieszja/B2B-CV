using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HortimexB2B.Infrastructure.Identity;
using HortimexB2B.Infrastructure.Repositories.Interfaces;
using HortimexB2B.Infrastructure.Services.Interfaces;
using HortimexB2B.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace HortimexB2B.Infrastructure.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly ICatalogRepository _catalogRepository;

        public CatalogService(ICatalogRepository catalogRepository)
        {
            _catalogRepository = catalogRepository;
        }

        public CatalogIndexViewModel GetCatalogItems(string productName, int pageIndex, int pageSize, int companyId)
        {
            var catalogViewModel = new CatalogIndexViewModel();
            catalogViewModel.CatalogItems = _catalogRepository.GetAllItems(productName, pageIndex, pageSize, companyId);

            var totalItems = _catalogRepository.GetTotalItems(productName, companyId);

            catalogViewModel.PaginationInfo = new PaginationInfoViewModel()
            {
                ActualPage = pageIndex,
                ItemsPerPage = pageSize,
                TotalItems = totalItems,
                TotalPages = int.Parse(Math.Ceiling(((decimal)totalItems / pageSize)).ToString())
            };

            catalogViewModel.Query = productName;
            catalogViewModel.PaginationInfo.Next = (catalogViewModel.PaginationInfo.ActualPage >= catalogViewModel.PaginationInfo.TotalPages) ? "disabled" : "";
            catalogViewModel.PaginationInfo.Previous = (catalogViewModel.PaginationInfo.ActualPage <= 1) ? "disabled" : "";

            return catalogViewModel;
        }

        public ItemViewModel GetItem(string itemId, int userId)
        {
            var vm = new ItemViewModel();

            var item = _catalogRepository.GetItem(itemId, userId);

            if (item == null)
                return null;

            var stock = _catalogRepository.GetItemStock(itemId);

            vm.Code = item.Code;
            vm.Name = item.Name;
            vm.IsDangerousGood = item.IsDangerousGood;
            vm.UnitPrice = item.UnitPrice;
            vm.Stock = stock;

            return vm;
        }

        public bool IsEnoughQuantityOnStock(string itemId, string unit, int demandQuantity)
        {
            var stock = _catalogRepository.GetItemStock(itemId).SingleOrDefault(x => x.Unit == unit);

            if (stock.Quantity >= demandQuantity)
            {
                return true;
            }

            return false;
        }
    }
}
