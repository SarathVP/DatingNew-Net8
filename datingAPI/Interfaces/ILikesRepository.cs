using datingAPI.DTO;
using datingAPI.Entities;
using datingAPI.Helpers;

namespace datingAPI.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike?> GetUserLike(int SourceUserId, int TargetUserId);
        Task<PagedList<MemberDto>> GetUserLikes(LikesParams likesParams);
        Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId);
        void DeleteLike(UserLike like);
        void AddLike(UserLike like);
    }
}