using HortimexB2B.Core.Domain.Order;
using HortimexB2B.Infrastructure.Identity;
using HortimexB2B.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HortimexB2B.Web.Pages.Orders
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly IFileService _fileService;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(IOrderService orderService, IFileService fileService, UserManager<ApplicationUser> userManager)
        {
            _orderService = orderService;
            _fileService = fileService;
            _userManager = userManager;
        }

        public List<Order> Model { get; set; } = new List<Order>();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await GetCurrentUserAsync();

            SetDefaultStartAndEndDate();

            Model = (await _orderService.GetOrdersAsync(user.GraffitiId, StartDate, EndDate)).ToList();
            return Page();
        }

        public async Task<IActionResult> OnGetFilterAsync(DateTime startDate, DateTime endDate)
        {
            var user = await GetCurrentUserAsync();

            StartDate = startDate;
            EndDate = endDate;

            //Platforma ma zwracać zamówienia nie starsze niż 2019-01-01
            var minDate = new DateTime(2019, 1, 1);
            if (StartDate < minDate)
            {
                StartDate = minDate;
            }

            Model = (await _orderService.GetOrdersAsync(user.GraffitiId, StartDate, EndDate)).ToList();
            return Page();
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

        public async Task<IActionResult> OnGetInvoice(int orderId, int orderYear)
        {
            var user = await _userManager.GetUserAsync(User);
            var vm = await _fileService.GetFile(orderId, orderYear, user.GraffitiId);

            if (vm == null)
                return NotFound();

            return File(new MemoryStream(vm.RawFile), vm.ContentType, vm.Name);
        }

        public async Task<IActionResult> OnGetFile(int documentId)
        {
            var vm = await _fileService.GetFile(documentId);

            if (vm == null)
                return NotFound();

            return File(new MemoryStream(vm.RawFile), vm.ContentType, vm.Name);
        }

        private void SetDefaultStartAndEndDate()
        {
            var now = DateTime.Now;
            StartDate = new DateTime(now.Year, now.Month, 1);
            EndDate = StartDate.AddMonths(1).AddDays(-1);
        }
    }
}