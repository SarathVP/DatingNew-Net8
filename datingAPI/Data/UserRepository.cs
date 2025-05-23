using AutoMapper;
using AutoMapper.QueryableExtensions;
using datingAPI.DTO;
using datingAPI.Entities;
using datingAPI.Helpers;
using datingAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace datingAPI.Data
{
    public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
    {
        public async Task<PagedList<MemberDto>> GetAllMembersAsync(UserParams userParams)
        {
            var query = context.Users.AsQueryable();
            query = query.Where(x => x.UserName != userParams.CurrentUserName);

            if (userParams.Gender != null)
            {
                query = query.Where(x => x.Gender == userParams.Gender);
            }
            var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge-1));
            var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));
            query = query.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);

            query = userParams.OrderBy switch{
                "created" => query.OrderByDescending(x => x.Created),
                _ => query.OrderByDescending(x => x.LastActive)
            };

            return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(mapper.ConfigurationProvider), userParams.PageNumber, userParams.PageSize);    
        }

        public async Task<MemberDto?> GetMemberByUsernameAsync(string username)
        {
            return await context.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<AppUser?> GetUserByIdAsync(int id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<AppUser?> GetUserByUsernameAsync(string username)
        {
            return await context.Users
                .Include(x => x.Photos)
                .SingleOrDefaultAsync(x=> x.UserName == username.ToLower());
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await context.Users
                .Include(x => x.Photos)
                .ToListAsync();
        }

        public void Update(AppUser appUser)
        {
            context.Entry(appUser).State = EntityState.Modified;
        }
    }
}