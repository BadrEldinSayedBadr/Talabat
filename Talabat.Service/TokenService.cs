using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.Service
{
    public class TokenService : ITokenService
    {
        #region old
        //private readonly IConfiguration configration;

        //public TokenService(IConfiguration configration)
        //{
        //    this.configration = configration;
        //}


        //public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager)
        //{

        //    //Private Claims [User-Defined]
        //    var authClaims = new List<Claim>()
        //    {
        //        new Claim(ClaimTypes.GivenName, user.DisplayName),
        //        new Claim(ClaimTypes.Email, user.Email)
        //    };

        //    var userRoles = await userManager.GetRolesAsync(user);

        //    foreach (var role in userRoles)
        //        authClaims.Add(new Claim(ClaimTypes.Role, role));

        //    //Security Key
        //    var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configration["JWT:Key"]));


        //    //Register Claims
        //    var token = new JwtSecurityToken(
        //        issuer: configration["JWT:ValidIssuer"],
        //        audience: configration["JWT:ValidAudience"],
        //        expires: DateTime.Now.AddDays(double.Parse(configration["JWT:DuratinInDays"])),
        //        claims: authClaims,
        //        signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
        //        );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //} 
        #endregion

        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
        }
        public string GenerateToken(AppUser appUser)
        {
            var clamis = new List<Claim>
            {
                new Claim(ClaimTypes.Email, appUser.Email),
                new Claim(ClaimTypes.GivenName, appUser.DisplayName)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(clamis),
                Issuer = _configuration["JWT:ValidIssuer"],
                IssuedAt = DateTime.Now,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }
    }
}
