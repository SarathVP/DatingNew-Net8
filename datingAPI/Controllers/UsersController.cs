using System.Security.Claims;
using AutoMapper;
using datingAPI.DTO;
using datingAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace datingAPI.Controllers
{
    [Authorize]
    public class UsersController(IUserRepository userRepository, IMapper mapper) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetAllUsers(){
            return Ok(await userRepository.GetAllMembersAsync());
        }
        
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username){
            var user = await userRepository.GetMemberByUsernameAsync(username);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto){
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (username == null) return BadRequest("No username found in token");

            var user = await userRepository.GetUserByUsernameAsync(username);
            if (user == null) return BadRequest("Could not find the user");

            mapper.Map(memberUpdateDto, user);
            if (await userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update the user");
        }
    }
}