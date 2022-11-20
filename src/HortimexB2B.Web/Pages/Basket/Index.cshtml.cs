using HortimexB2B.Infrastructure.Identity;
using HortimexB2B.Infrastructure.Services.Interfaces;
using HortimexB2B.Web.ViewModels;
using HortimexB2B.Web.ViewModels.Basket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HortimexB2B.Web.Pages.Basket
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IBasketService _basketService;
        private readonly ICatalogService _catalogService;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(IBasketService basketService, ICatalogService catalogService, UserManager<ApplicationUser> userManager)
        {
            _basketService = basketService;
            _catalogService = catalogService;
            _userManager = userManager;
        }

        public BasketViewModel BasketModel { get; set; } = new BasketViewModel();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await GetCurrentUserAsync();
            var basket = await _basketService.GetAsync(Guid.Parse(user.Id));

            if (basket != null)
            {
                BasketModel.BasketId = basket.BasketId;
                BasketModel.UserId = basket.UserId;
                BasketModel.Items = basket.BasketItems.Select(i =>
                {
                    var itemModel = new BasketItemViewModel()
                    {
                        ItemId = i.ItemId,
                        UnitPrice = i.UnitPrice,
                        Vat = i.Vat,
                        Quantity = i.Quantity,
                        ProductId = i.ProductId,
                        ProductName = i.ProductName,
                        Unit = i.Unit,
                        UnitName = i.UnitName,
                        ConvertedQuantity = i.ConvertedQuantity
                    };
                    return itemModel;
                }).ToList();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CatalogItemViewModel productItem)
        {
            var isEnough = _catalogService.IsEnoughQuantityOnStock(productItem.Code, productItem.Unit, productItem.Quantity);

            if (!isEnough)
            {
                ModelState.AddModelError("stock", "Demand quantity is greater than stock");
            }

            if (!ModelState.IsValid)
            {
                return Redirect(Request.Headers["Referer"]);
            }

            var user = await GetCurrentUserAsync();
            await _basketService.AddItemAsync(Guid.Parse(user.Id), user.GraffitiId, productItem.Code, productItem.Name, productItem.UnitPrice, productItem.Unit, productItem.Quantity, productItem.StockId);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateAsync(Dictionary<string, int> items)
        {
            if (items != null)
            {
                var user = await GetCurrentUserAsync();
                var basket = await _basketService.GetAsync(Guid.Parse(user.Id));
                await _basketService.SetQuantitiesAsync(basket.BasketId, items);
            }
            return RedirectToPage();
        }

        public async Task<ActionResult> OnGetDeleteAsync(Guid id)
        {
            if (id != null)
            {
                var user = await GetCurrentUserAsync();
                await _basketService.DeleteItemAsync(Guid.Parse(user.Id), id);
            }
            return RedirectToPage();
        }

        public async Task<ActionResult> OnGetDeleteBasketAsync(Guid basketId)
        {
            if (basketId != null)
            {
                var user = await GetCurrentUserAsync();
                await _basketService.DeleteBasketAsync(Guid.Parse(user.Id), basketId);
            }
            return RedirectToPage();
        }

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return user;
        }
    }
}