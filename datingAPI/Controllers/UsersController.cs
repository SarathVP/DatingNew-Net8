using datingAPI.DTO;
using datingAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace datingAPI.Controllers
{
    public class UsersController(IUserRepository userRepository) : BaseApiController
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
    }
}