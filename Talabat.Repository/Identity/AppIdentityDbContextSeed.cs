using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> _userManager)
        {
            if (_userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    DisplayName="Michael Nader",
                    Email="michaelnadersaad@gmail.com",
                    NormalizedUserName="michaelnadersaad",
                    PhoneNumber="01204940411"
                };
                await _userManager.CreateAsync(user,"Mico123$");
            }
        }
    }
}
