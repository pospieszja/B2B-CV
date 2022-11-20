using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HortimexB2B.Core.Domain;
using HortimexB2B.Infrastructure.Identity;
using HortimexB2B.Infrastructure.Services.Interfaces;
using HortimexB2B.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HortimexB2B.Web.Pages.Catalog
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ICatalogService _catalogService;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(ICatalogService catalogService, UserManager<ApplicationUser> userManager)
        {
            _catalogService = catalogService;
            _userManager = userManager;
        }

        public CatalogIndexViewModel CatalogModel { get; set; } = new CatalogIndexViewModel();

        public async Task<IActionResult> OnGetAsync([FromQuery(Name = "pageid")]int pageIndex = 1, [FromQuery(Name = "query")]string query = "")
        {
            var user = await _userManager.GetUserAsync(User);


            if (pageIndex < 1)
                return RedirectToPage("./Index");

            if (String.IsNullOrEmpty(query))
            {
                CatalogModel = _catalogService.GetCatalogItems("", pageIndex, 12, user.GraffitiId);
            }
            else
            {
                CatalogModel = _catalogService.GetCatalogItems(query.Trim(), pageIndex, 12, user.GraffitiId);
            }

            if (CatalogModel.CatalogItems.Count() == 0)
            {
                return Page();
            }

            if (pageIndex > CatalogModel.PaginationInfo.TotalPages)
                return RedirectToPage("./Index");

            return Page();
        }
    }
}