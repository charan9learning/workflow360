using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WorkFlow360.Application.Common.Interface;
using WorkFlow360.Domain.Entities;

namespace WorkFlow360.Infrastructure.Authentication
{
    internal sealed class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings _settings;

        public JwtTokenGenerator(IOptions<JwtSettings> options)
        {
            _settings = options.Value;
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new(ClaimTypes.Role, user.Role)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_settings.ExpirationMinutes);

            var token = new JwtSecurityToken(issuer: _settings.Issuer, audience: _settings.Audience, claims: claims, expires: expires, signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
