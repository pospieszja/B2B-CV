using HortimexB2B.Infrastructure.Services.Interfaces;
using HortimexB2B.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace HortimexB2B.Web.Pages.Admin
{
    [Authorize(Roles = "Administrator")]
    public class AddUserModel : PageModel
    {
        [BindProperty]
        public AddUserViewModel AddUser { get; set; }

        private readonly IUserService _userService;

        public AddUserModel(IUserService userService)
        {
            _userService = userService;
        }

        public void OnGet()
        {
        }

        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var response = await _userService.AddUserAsync(AddUser);

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