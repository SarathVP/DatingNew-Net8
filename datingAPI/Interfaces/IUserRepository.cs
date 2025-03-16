using datingAPI.DTO;
using datingAPI.Entities;

namespace datingAPI.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser appUser);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser?> GetUserByIdAsync(int id);
        Task<AppUser?> GetUserByUsernameAsync(string username);
        Task<MemberDto?> GetMemberByUsernameAsync(string username);
        Task<IEnumerable<MemberDto>> GetAllMembersAsync();
    }
}