

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using datingAPI.Entities;
using datingAPI.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace datingAPI.Services
{
    public class TokenService(IConfiguration configuration) : ITokenService
    {
        public string GenerateToken(AppUser appUser)
        {
            var TokenKey = configuration.GetValue<string>("TokenKey:Key") ?? throw new Exception("No Token Key found");
            if (TokenKey.Length < 64) throw new Exception("Token length is lesser than 64");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenKey));
            var claims = new List<Claim>{
                new(ClaimTypes.NameIdentifier, appUser.UserName)
            };
            var signCreds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var TokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = signCreds
            };
            var TokenHandler = new JwtSecurityTokenHandler();
            var Token = TokenHandler.CreateToken(TokenDescriptor);
            return TokenHandler.WriteToken(Token);
        }
    }
}