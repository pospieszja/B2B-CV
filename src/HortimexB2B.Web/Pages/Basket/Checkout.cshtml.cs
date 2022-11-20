using HortimexB2B.Core.Domain.Order;
using HortimexB2B.Infrastructure.Helpers;
using HortimexB2B.Infrastructure.Identity;
using HortimexB2B.Infrastructure.Repositories.Interfaces;
using HortimexB2B.Infrastructure.Services.Interfaces;
using HortimexB2B.Web.ViewModels.Basket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HortimexB2B.Web.Pages.Basket
{
    [Authorize]
    public class CheckoutModel : PageModel
    {
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;
        private readonly ICatalogService _catalogService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICompanyRepository _companyRepository;

        [BindProperty]
        public CheckoutViewModel Model { get; set; } = new CheckoutViewModel();

        public CheckoutModel(IBasketService basketService, IOrderService orderService, ICatalogService catalogService, ICompanyRepository companyRepository, UserManager<ApplicationUser> userManager)
        {
            _basketService = basketService;
            _orderService = orderService;
            _catalogService = catalogService;
            _userManager = userManager;
            _companyRepository = companyRepository;
        }

        public async Task<IActionResult> OnPostBuyAsync()
        {
            if (!DesiredDeliveryDateIsValid())
            {
                ModelState.AddModelError("", $"Wybrana planowana data dostawy ({Model.DesiredDeliveryDate.ToString("yyyy-MM-dd")}) jest nieprawidłowa");
                await PrepareViewModel();
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);

            var userId = Guid.Parse(user.Id);
            var basket = await _basketService.GetAsync(userId);
            var salesManager = _companyRepository.GetSalesManagerInfo(user.GraffitiId);

            await _orderService.CreateOrdersAsync(salesManager.OrderRegionId, user.GraffitiId, Model.SelectedAddress, Model.ReferenceNumber, Model.Comments, Model.DesiredDeliveryDate, basket);

            await _basketService.DeleteBasketAsync(userId, basket.BasketId);

            return RedirectToPage("CheckoutSuccess");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            var userId = Guid.Parse(user.Id);
            var basket = await _basketService.GetAsync(userId);

            await PrepareViewModel();

            return Page();
        }

        private async Task PrepareViewModel()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = Guid.Parse(user.Id);

            var basket = await _basketService.GetAsync(userId);
            var shippingAddressList = _orderService.GetAllShippingAddress(user.GraffitiId);

            Model.Addresses = shippingAddressList.Select(shippingAddress => new ShippingAddressViewModel
            {
                Id = shippingAddress.Id,
                City = shippingAddress.City,
                Name = shippingAddress.Name,
                Post = shippingAddress.Post,
                PostalCode = shippingAddress.PostalCode,
                Street = shippingAddress.Street
            });

            Model.Basket = new BasketViewModel
            {
                BasketId = basket.BasketId,
                Items = basket.BasketItems.Select(basketItem => new BasketItemViewModel
                {
                    ConvertedQuantity = basketItem.ConvertedQuantity,
                    ItemId = basketItem.ItemId,
                    ProductId = basketItem.ProductId,
                    ProductName = basketItem.ProductName,
                    Quantity = basketItem.Quantity,
                    Unit = basketItem.Unit,
                    UnitPrice = basketItem.UnitPrice,
                    Vat = basketItem.Vat
                }).ToList(),
                UserId = basket.UserId
            };

            BuildAddressForSelectList(shippingAddressList);
            PrepareMinimumDeliveryDate();
        }

        private void PrepareMinimumDeliveryDate()
        {
            var today = DateTime.Now;

            if (today.Hour >= 10 || HolidayCalculator.IsHoliday(today))
            {
                Model.DesiredDeliveryDate = NextPossibleDeliveryDay(today.AddDays(1), 2);
                Model.MinDesiredDeliveryDate = Model.DesiredDeliveryDate;
                return;
            }

            Model.MinDesiredDeliveryDate = NextPossibleDeliveryDay(today.AddDays(1), 1);
        }

        private void BuildAddressForSelectList(IEnumerable<ShippingAddress> shippingAddressList)
        {
            Model.AddressSelectList = shippingAddressList.Select(address => new SelectListItem
            {
                Text = String.Join(" ", address.Name, address.Street, address.City, address.PostalCode),
                Value = address.Id.ToString()
            }).ToList();
        }
        private DateTime NextPossibleDeliveryDay(DateTime day, int requiredDays)
        {
            var workingDaysCount = 0;

            while (true)
            {
                if (!HolidayCalculator.IsHoliday(day))
                {
                    workingDaysCount++;
                }
                if (workingDaysCount == requiredDays)
                {
                    return day;
                }
                day = day.AddDays(1);
            }
        }

        private bool DesiredDeliveryDateIsValid()
        {
            if (Model.DesiredDeliveryDate >= Model.MinDesiredDeliveryDate && !HolidayCalculator.IsHoliday(Model.DesiredDeliveryDate))
            {
                return true;
            }

            return false;
        }
    }
}