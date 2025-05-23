using datingAPI.DTO;
using datingAPI.Entities;
using datingAPI.Helpers;

namespace datingAPI.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser appUser);
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser?> GetUserByIdAsync(int id);
        Task<AppUser?> GetUserByUsernameAsync(string username);
        Task<MemberDto?> GetMemberByUsernameAsync(string username);
        Task<PagedList<MemberDto>> GetAllMembersAsync(UserParams userParams);
    }
}