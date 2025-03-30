using datingAPI.Data;
using datingAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace datingAPI.Controllers
{
    public class BuggyController(DataContext context) : BaseApiController
    {
        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetAuthError(){
            return "You are not authorized";
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound(){
            var user = context.Users.Find(-1);
            if (user == null) return NotFound();
            return user;
        }

        [HttpGet("server-error")]
        public ActionResult<AppUser> GetServerError(){
            var user = context.Users.Find(-1) ?? throw new Exception("This is a Server error");
            return user;
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest(){
            return BadRequest("This is a bad request");
        }
    }
}