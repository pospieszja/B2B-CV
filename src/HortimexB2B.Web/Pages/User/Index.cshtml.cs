using System.Threading.Tasks;
using HortimexB2B.Infrastructure.Identity;
using HortimexB2B.Infrastructure.Repositories.Interfaces;
using HortimexB2B.Web.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HortimexB2B.Web.Pages.User
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICompanyRepository _companyRepository;

        public IndexModel(UserManager<ApplicationUser> userManager, ICompanyRepository companyRepository)
        {
            _userManager = userManager;
            _companyRepository = companyRepository;
        }

        [BindProperty]
        public UserInfoViewModel UserInfoViewModel { get; set; } = new UserInfoViewModel();

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            var company = _companyRepository.GetCompany(user.GraffitiId);
            var companyAddress = _companyRepository.GetAddress(user.GraffitiId);
            var companyBilling = _companyRepository.GetBilling(user.GraffitiId);
            var salesManager = _companyRepository.GetSalesManagerInfo(user.GraffitiId);

            if (company != null)
            {
                UserInfoViewModel.GraffitiID = user.GraffitiId.ToString();
                UserInfoViewModel.CompanyName = company.Name;
                UserInfoViewModel.NIP = company.NIP;
                UserInfoViewModel.Address.City = companyAddress.City;
                UserInfoViewModel.Address.Street = companyAddress.Street;
                UserInfoViewModel.Address.PostalCode = companyAddress.PostalCode;
                UserInfoViewModel.PaymentTerms = companyBilling.PaymentTerms;
                UserInfoViewModel.CreditLimit = companyBilling.CreditLimit;
                UserInfoViewModel.SalesManager.Email = salesManager.Email;
                UserInfoViewModel.SalesManager.Name = salesManager.Name;
                UserInfoViewModel.SalesManager.Phone = salesManager.Phone;
            }
            UserInfoViewModel.Email = user.Email;


            return Page();
        }
    }
}