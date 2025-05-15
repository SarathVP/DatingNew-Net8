using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using datingAPI.Entities;
using datingAPI.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace datingAPI.Services
{
    public class TokenService(IConfiguration configuration, UserManager<AppUser> userManager) : ITokenService
    {
        public async Task<string> GenerateToken(AppUser appUser)
        {
            var TokenKey = configuration.GetValue<string>("TokenKey:Key") ?? throw new Exception("No Token Key found");
            if (TokenKey.Length < 64) throw new Exception("Token length is lesser than 64");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenKey));

            if(appUser.UserName == null) throw new Exception("No Username for user");

            var claims = new List<Claim>{
                new(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
                new(ClaimTypes.Name, appUser.UserName)
            };

            var roles = await userManager.GetRolesAsync(appUser);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));


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