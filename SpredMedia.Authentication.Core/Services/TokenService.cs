using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SpredMedia.Authentication.Core.Interface;
using SpredMedia.Authentication.Model.model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace SpredMedia.Authentication.Core.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _manager;

        public TokenService(IConfiguration configuration, UserManager<User> manager)
        {
            _configuration = configuration;
            _manager = manager;
        }
        public async Task<string> GenerateToken(User user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
            };
            var roles = await _manager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Token")));
            var jwtConfig = _configuration.GetSection("Jwt");
            var token = new JwtSecurityToken
            (
                issuer: _configuration.GetValue<string>("Jwt:Issuer"),
                audience: _configuration.GetValue<string>("Jwt:Audience"),
                claims: authClaims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtConfig.GetSection("lifetime").Value)),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha512Signature
             ));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
