using datingAPI.DTO;
using datingAPI.Entities;
using datingAPI.Extensions;
using datingAPI.Helpers;
using datingAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace datingAPI.Controllers
{
    public class LikesController(ILikesRepository likesRepository) : BaseApiController
    {
        [HttpPost("{targetuserid:int}")]
        public async Task<ActionResult> ToggleLike(int targetuserid)
        {
            var sourceUserId = User.GetUserId();
            if(sourceUserId == targetuserid) return BadRequest("You cannot like user self");

            var existingLike = await likesRepository.GetUserLike(sourceUserId,targetuserid);

            if(existingLike == null){
                var like = new UserLike{
                    SourceUserId = sourceUserId,
                    TargetUserId = targetuserid
                };
                likesRepository.AddLike(like);
            }else
            {
                likesRepository.DeleteLike(existingLike);
            }
            if (await likesRepository.SaveChanges()) return Ok();

            return BadRequest("Failed to update like");
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<int>>> GetCurrentUserLikeIds()
        {
            return Ok(await likesRepository.GetCurrentUserLikeIds(User.GetUserId()));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUserLikes([FromQuery] LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
            var users = await likesRepository.GetUserLikes(likesParams);

            Response.AddPaginationHeader(users);
            return Ok(users);
        }
    }
}