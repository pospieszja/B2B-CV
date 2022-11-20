using HortimexB2B.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HortimexB2B.Infrastructure.Services.Interfaces
{
    public interface IUserService
    {
        IEnumerable<UserViewModel> GetUsers();
        Task<IdentityResult> AddUserAsync(AddUserViewModel user);
        Task DeleteUserAsync(string userId);
        Task<EditUserViewModel> GetUser(string userId);
        Task<IdentityResult> UpdateUserAsync(EditUserViewModel user);
        Task<bool> IsConsentWasGivenAsync(string userName);
        Task GiveConsentAsync(string userName);
    }
}
