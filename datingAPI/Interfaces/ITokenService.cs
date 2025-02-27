

using datingAPI.Entities;

namespace datingAPI.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(AppUser appUser);
    }
}