using AutoMapper;
using datingAPI.DTO;
using datingAPI.Entities;
using datingAPI.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace datingAPI.Controllers
{
    public class AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper)
             : BaseApiController
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("User Already Exists");

            var user = mapper.Map<AppUser>(registerDto);
            user.UserName = registerDto.Username.ToLower();

            var result = await userManager.CreateAsync(user, registerDto.PassWord);

            if (!result.Succeeded) return BadRequest(result.Errors);

            return new UserDto{
                Username = user.UserName,
                Token = await tokenService.GenerateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
            
        }

        private async Task<bool> UserExists(string userName){
            return await userManager.Users.AnyAsync(x => x.NormalizedUserName == userName.ToUpper());
        }

       [HttpPost("login")]
       public async Task<ActionResult<UserDto>> Login(LoginDto loginDto){
        var user = await userManager.Users
                .Include(x => x.Photos)
                .FirstOrDefaultAsync(x => x.NormalizedUserName == loginDto.UserName.ToUpper());

        if (user == null || user.UserName == null) return Unauthorized("User Not Found");

        var result = await userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!result) return Unauthorized();

        return new UserDto{
            Username = user.UserName,
            KnownAs = user.KnownAs,
            Token = await tokenService.GenerateToken(user),
            PhotoUrl = user.Photos?.FirstOrDefault(x => x.IsMain)?.Url,
            Gender = user.Gender
        };
       }
    }
}