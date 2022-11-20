using HortimexB2B.Infrastructure.Identity;
using HortimexB2B.Infrastructure.Settings;
using HortimexB2B.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace HortimexB2B.Web.Configuration
{
    public static class SeedData
    {
        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var context = serviceProvider.GetRequiredService<AppIdentityDbContext>();

            var settings = serviceProvider.GetRequiredService<IOptions<AdminSettings>>().Value;

            string[] roleNames = { "Administrator", "Customer" };
            IdentityResult roleResult;


            context.Database.EnsureCreated();

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);

                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var user = await userManager.FindByEmailAsync(settings.User);

            if (user == null)
            {
                var appUser = new ApplicationUser
                {
                    UserName = settings.User,
                    Email = settings.User,
                };
                string adminPassword = settings.Password;

                var createPowerUser = await userManager.CreateAsync(appUser, adminPassword);
                if (createPowerUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(appUser, "Administrator");
                }
            }
        }
    }
}
