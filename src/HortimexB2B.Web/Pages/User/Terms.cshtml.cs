using HortimexB2B.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace HortimexB2B.Web
{
    [Authorize]
    public class TermsModel : PageModel
    {
        [Range(typeof(bool), "true", "true", ErrorMessage = "Regulamin Platformy dla Odbiorców - pole wymagane!")]
        [BindProperty]
        public bool PlatformTerms { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "Ogólne warunki sprzedaży - pole wymagane!")]
        [BindProperty]
        public bool SaleTerms { get; set; }

        private readonly IUserService _userService;

        public TermsModel(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> OnGet()
        {
            bool consentGiven = await _userService.IsConsentWasGivenAsync(User.Identity.Name);

            if (consentGiven)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }

        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _userService.GiveConsentAsync(User.Identity.Name);

            return RedirectToPage("/Index");
        }
    }
}