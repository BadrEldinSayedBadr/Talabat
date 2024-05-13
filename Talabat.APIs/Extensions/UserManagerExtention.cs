using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Extensions
{
    public static class UserManagerExtention
    {
        public static async Task<AppUser> FindUserWithAddressByEmail (this UserManager<AppUser> userManager, ClaimsPrincipal CurrentUser)
        {
            var email = CurrentUser.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }
    }
}
