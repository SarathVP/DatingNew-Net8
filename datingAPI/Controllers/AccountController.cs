using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using datingAPI.Data;
using datingAPI.DTO;
using datingAPI.Entities;
using datingAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace datingAPI.Controllers
{
    public class AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
             : BaseApiController
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("User Already Exists");

            using var hmac = new HMACSHA512();

            var user = mapper.Map<AppUser>(registerDto);
            user.UserName = registerDto.Username;
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.PassWord));
            user.PasswordSalt = hmac.Key;

            context.Users.Add(user);
            await context.SaveChangesAsync();
            return new UserDto{
                Username = user.UserName,
                Token = tokenService.GenerateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
            
        }

        private async Task<bool> UserExists(string userName){
            return await context.Users.AnyAsync(x => x.UserName.ToLower() == userName.ToLower());
        }

       [HttpPost("login")]
       public async Task<ActionResult<UserDto>> login(LoginDto loginDto){
        var user = await context.Users
                .Include(x => x.Photos)
                .FirstOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());

        if (user == null) return Unauthorized("User Not Found");

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var passwordhash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < passwordhash.Length; i++)
        {
            if (passwordhash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
        }
        

        return new UserDto{
            Username = user.UserName,
            KnownAs = user.KnownAs,
            Token = tokenService.GenerateToken(user),
            PhotoUrl = user.Photos?.FirstOrDefault(x => x.IsMain)?.Url,
            Gender = user.Gender
        };
       }
    }
}