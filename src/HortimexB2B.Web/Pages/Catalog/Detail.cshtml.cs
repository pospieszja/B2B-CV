using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class DetailModel : PageModel
    {
        private readonly ICatalogService _catalogService;
        private readonly IFileService _fileService;
        private readonly UserManager<ApplicationUser> _userManager;

        public DetailModel(ICatalogService catalogService, IFileService fileService, UserManager<ApplicationUser> userManager)
        {
            _catalogService = catalogService;
            _fileService = fileService;
            _userManager = userManager;
        }

        public ItemViewModel ItemModel { get; set; } = new ItemViewModel();

        public async Task<IActionResult> OnGet(string itemId)
        {
            var user = await _userManager.GetUserAsync(User);
            ItemModel = _catalogService.GetItem(itemId, user.GraffitiId);

            if (ItemModel == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnGetDownload(string itemId, int documentType)
        {
            var vm = new FileViewModel();
            vm = await _fileService.GetFile(itemId, documentType);

            if (vm.RawFile == null)
                return NotFound();

            return File(vm.RawFile, vm.ContentType, vm.Name);
        }
    }
}