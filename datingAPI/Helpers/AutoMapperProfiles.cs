using AutoMapper;
using datingAPI.DTO;
using datingAPI.Entities;
using datingAPI.Extensions;

namespace datingAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(d => d.Age, f => f.MapFrom(s => s.DateOfBirth.CalculateAge()))
                .ForMember(d => d.PhotoUrl, f => f.MapFrom(s => s.Photos!.FirstOrDefault(i => i.IsMain)!.Url));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, AppUser>();
        }
    }
}