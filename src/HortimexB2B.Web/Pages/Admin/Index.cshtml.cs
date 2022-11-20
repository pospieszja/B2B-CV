using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HortimexB2B.Infrastructure.Services.Interfaces;
using HortimexB2B.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HortimexB2B.Web.Pages.Admin
{
    [Authorize(Roles = "Administrator")]
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;

        [BindProperty]
        public EditUserViewModel EditUser { get; set; }

        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        public IEnumerable<UserViewModel> UsersListViewModel { get; set; } = new List<UserViewModel>();

        public void OnGet()
        {
            UsersListViewModel = _userService.GetUsers().OrderBy(u => u.CompanyName);
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            await _userService.DeleteUserAsync(id);
            return RedirectToPage("/Admin/Index");
        }

        public async Task<IActionResult> OnGetEditAsync(string id)
        {
            EditUser = await _userService.GetUser(id);
            return RedirectToPage("/Admin/Index");
        }
    }
}