using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;
using Talabat.Core.ServiceInterfaces;

namespace Talabat.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configration;
        public TokenService(IConfiguration configration)
        {
            _configration = configration;
        }
        public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> _userManager)
        {
            var authClaims = new List<Claim>(){
                new Claim(ClaimTypes.GivenName, user.DisplayName),
                new Claim(ClaimTypes.Email, user.Email)
            };
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configration["JWT:Key"]));
            var token = new JwtSecurityToken(
                issuer: _configration["JWT:ValidIssuer"],
                audience: _configration["JWT:ValidAudience"],
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature),
                expires: DateTime.Now.AddDays(double.Parse(_configration["JWT:DurationDays"])
                ));
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
