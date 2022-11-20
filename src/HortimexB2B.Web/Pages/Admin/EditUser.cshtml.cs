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
    public class EditUserModel : PageModel
    {
        private readonly IUserService _userService;

        [BindProperty]
        public EditUserViewModel EditUser { get; set; }

        public EditUserModel(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            EditUser = await _userService.GetUser(userId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string userId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var response = await _userService.UpdateUserAsync(EditUser);

            if (!response.Succeeded)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return Page();
            }

            return RedirectToPage("/Admin/Index");
        }

    }
}
