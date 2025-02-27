

using System.Security.Cryptography;
using System.Text;
using datingAPI.Data;
using datingAPI.DTO;
using datingAPI.Entities;
using datingAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace datingAPI.Controllers
{
    public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("User Already Exists");
            using var hmac = new HMACSHA512();
            var user = new AppUser{
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.passWord)),
                PasswordSalt = hmac.Key
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return new UserDto{
                Username = user.UserName,
                Token = tokenService.GenerateToken(user)
            };
            
        }

        private async Task<bool> UserExists(string userName){
            return await context.Users.AnyAsync(x => x.UserName.ToLower() == userName.ToLower());
        }

       [HttpPost("login")]
       public async Task<ActionResult<UserDto>> login(LoginDto loginDto){
        var user = await context.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());

        if (user == null) return Unauthorized("User Not Found");

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var passwordhash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < passwordhash.Length; i++)
        {
            if (passwordhash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
        }

        return new UserDto{
            Username = user.UserName,
            Token = tokenService.GenerateToken(user)
        };
       }
    }
}