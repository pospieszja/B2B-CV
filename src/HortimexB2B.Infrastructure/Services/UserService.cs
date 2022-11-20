using HortimexB2B.Infrastructure.Identity;
using HortimexB2B.Infrastructure.Repositories.Interfaces;
using HortimexB2B.Infrastructure.Services.Interfaces;
using HortimexB2B.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HortimexB2B.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICompanyRepository _companyRepository;
        private readonly INotificationService _notificationService;
        private readonly ITermConsentRepository _termConsentRepository;

        public UserService(UserManager<ApplicationUser> userManager, ICompanyRepository companyRepository, INotificationService notificationService, ITermConsentRepository termConsentRepository)
        {
            _userManager = userManager;
            _companyRepository = companyRepository;
            _notificationService = notificationService;
            _termConsentRepository = termConsentRepository;
        }

        public async Task<IdentityResult> AddUserAsync(AddUserViewModel userVM)
        {
            var company = _companyRepository.GetCompany(userVM.GraffitiId);

            if (company == null)
            {
                throw new Exception($"Company with: '{userVM.GraffitiId}' id was not found in Graffiti database.");
            }

            var user = new ApplicationUser();

            user.GraffitiId = userVM.GraffitiId;
            user.Email = userVM.Email;
            user.UserName = userVM.Email;

            var result = await _userManager.CreateAsync(user, userVM.Password);

            if (result.Succeeded)
            {
                var tags = new Dictionary<string, string>();
                tags.Add("password", userVM.Password);
                tags.Add("email", userVM.Email);
                await _notificationService.BroadcastAsync(user, tags, "WELCOME", "PL");

                result = await _userManager.AddToRoleAsync(user, "Customer");
            }

            return result;
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.DeleteAsync(user);
        }

        public async Task<EditUserViewModel> GetUser(string userId)
        {
            var vm = new EditUserViewModel();

            var user = await _userManager.FindByIdAsync(userId);

            vm.Id = user.Id;
            vm.Email = user.Email;
            vm.GraffitiId = user.GraffitiId;

            return vm;
        }

        public IEnumerable<UserViewModel> GetUsers()
        {
            var vm = new List<UserViewModel>();

            //TODO: Make it better - it seems to be ugly solution
            vm = _userManager.Users.Where(u => (u.Email != "admin@hortimex.pl" && u.Email != "admin.platforma@hortimex.pl"))
                        .Select(u => new UserViewModel
                        {
                            GraffitiId = u.GraffitiId,
                            CompanyName = _companyRepository.GetCompany(u.GraffitiId).ShortName,
                            Email = u.Email,
                            Id = u.Id,
                            NIP = _companyRepository.GetCompany(u.GraffitiId).NIP
                        }).ToList();

            return vm;
        }

        public async Task GiveConsentAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            await _termConsentRepository.AddConsentAsync(userName);
            await _notificationService.BroadcastAsync(user, "FIRST_LOGIN", "PL");
        }

        public async Task<bool> IsConsentWasGivenAsync(string userName)
        {
            var consent = await _termConsentRepository.GetByUserAsync(userName);

            if (consent != null)
            {
                return true;
            }
            return false;
        }

        public async Task<IdentityResult> UpdateUserAsync(EditUserViewModel userVM)
        {
            var company = _companyRepository.GetCompany(userVM.GraffitiId);

            if (company == null)
            {
                throw new Exception($"Company with: '{userVM.GraffitiId}' id was not found in Graffiti database.");
            }

            var user = await _userManager.FindByIdAsync(userVM.Id);

            user.Email = userVM.Email;
            user.UserName = userVM.Email;
            user.GraffitiId = userVM.GraffitiId;

            if (!String.IsNullOrEmpty(userVM.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, userVM.Password);

                if (!result.Succeeded)
                    return result;
            }

            return await _userManager.UpdateAsync(user);
        }
    }
}
