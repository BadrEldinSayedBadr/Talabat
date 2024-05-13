using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Data.Context
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var User = new AppUser()
                {
                    DisplayName = "Badr Eldin",
                    Email = "badreldin@gmail.com",
                    UserName = "badr.eldin",
                    PhoneNumber = "01062023880"
                };

                await userManager.CreateAsync(User, "P@ssw0rd");
            }
        }
    }
}
