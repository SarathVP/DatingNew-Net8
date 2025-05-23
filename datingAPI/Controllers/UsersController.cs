using AutoMapper;
using datingAPI.DTO;
using datingAPI.Entities;
using datingAPI.Extensions;
using datingAPI.Helpers;
using datingAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace datingAPI.Controllers
{
    [Authorize]
    public class UsersController(IUnitOfWork unitOfWork, IMapper mapper,
                    IPhotoService photoService) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetAllUsers([FromQuery] UserParams userParams){
            userParams.CurrentUserName = User.GetUsername();

            var users = await unitOfWork.UserRepository.GetAllMembersAsync(userParams);
            Response.AddPaginationHeader(users);
            return Ok(users);

        }
        
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username){
            var user = await unitOfWork.UserRepository.GetMemberByUsernameAsync(username);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto){

            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user == null) return BadRequest("Could not find the user");

            mapper.Map(memberUpdateDto, user);
            if (await unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to update the user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file){
            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user == null) return BadRequest("Could not update the user");

            var result = await photoService.AddPhotoAsync(file);
            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo{
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };
            if (user.Photos!.Count == 0) photo.IsMain = true;
            user.Photos?.Add(photo);

            if (await unitOfWork.Complete())
             return CreatedAtAction(nameof(GetUser), new {username = user.UserName}, mapper.Map<PhotoDto>(photo));

            return BadRequest("Problem in adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId){
            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user == null) return BadRequest("Could not find the user");

            var photo = user.Photos?.FirstOrDefault(x => x.Id == photoId);
            if (photo == null || photo.IsMain) return BadRequest("Cannot update this as Main photo");

            var currentMain = user.Photos?.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;

            photo.IsMain = true;

            if (await unitOfWork.Complete()) return NoContent();

            return BadRequest("Problem while setting up the main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId){
            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user == null) return BadRequest("Could not find the user");

            var photo = user.Photos?.FirstOrDefault(x => x.Id == photoId);
            if (photo == null || photo.IsMain) return BadRequest("Cannot delete this photo");

            if (photo.PublicId != null)
            {
                var result = await photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }
            user.Photos?.Remove(photo);
            if (await unitOfWork.Complete()) return Ok();

            return BadRequest("Problem while deleting the photo");
        }
    }
}