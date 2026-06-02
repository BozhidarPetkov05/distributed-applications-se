using CarSales.Contracts.Interfaces;
using CarSales.Contracts.Settings;
using CarSales.Data.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarSales.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _settings;

        public TokenService(IOptions<JwtSettings> options)
        {
            _settings = options.Value;
        }

        public string CreateToken(User user)
        {
            Claim[] claims = new Claim[]
            {
                new Claim("loggedUserId", user.Id.ToString()),
                new Claim("isAdmin", user.IsAdmin.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken
            (
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_settings.ExpirationHours),
                signingCredentials: credentials
            );

            string tokenData = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenData;
        }
    }

}
