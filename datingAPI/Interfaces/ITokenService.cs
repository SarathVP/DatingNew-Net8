

using datingAPI.Entities;

namespace datingAPI.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateToken(AppUser appUser);
    }
}