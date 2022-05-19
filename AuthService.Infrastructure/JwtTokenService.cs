using AuthService.Domain;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Infrastructure
{
    
    public class JwtTokenService : ITokenService
    {
        public string Create(User user)
        {
            string secretKey = "your-256-bit-secret";

            string issuer = "Vavatech";

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, user.LastName),

                new Claim(ClaimTypes.Role, "Trainer"),
                new Claim(ClaimTypes.Role, "Developer"),

                new Claim(ClaimTypes.DateOfBirth, user.Birthday.ToShortDateString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // dotnet add package System.IdentityModel.Tokens.Jwt
            var securityToken = new JwtSecurityToken(
                issuer: issuer,
                audience: issuer,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(securityToken);

            return token;
        }
    }
}