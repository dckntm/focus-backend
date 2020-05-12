using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Focus.Service.Identity.Application.Services;
using Focus.Service.Identity.Core.Enums;
using Microsoft.IdentityModel.Tokens;

namespace Focus.Service.Identity.Infrastructure.Security
{
    public class JwtSecurityTokenGenerator : ISecurityTokenGenerator
    {
        // TODO: think of injecting key via env variable
        public string Generate(string username, UserRole role, string orgId)
        {
            var claims = new[] {
                new Claim("name", username),
                new Claim("role", role.Value()),
                new Claim("org", orgId)
            };

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("Amr273YaMvDu4X5WEvG2jmwsdaJY3ADRT6hFeZvXHhMD7nt6Bd"));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "focus_issuer",
                audience: "focus_audience",
                claims: claims,
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}