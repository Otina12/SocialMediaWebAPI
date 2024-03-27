using Microsoft.IdentityModel.Tokens;
using SocialMediaWebApp.Interfaces;
using SocialMediaWebApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SocialMediaWebApp.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]!));
        }

        public string CreateToken(Member member)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, member.Id),
                new Claim(JwtRegisteredClaimNames.Email, member.Email!),
                new Claim(JwtRegisteredClaimNames.GivenName, member.UserName!)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
