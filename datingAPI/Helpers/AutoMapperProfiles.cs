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
            CreateMap<RegisterDto, AppUser>();
            CreateMap<string, DateOnly>().ConvertUsing(s => DateOnly.Parse(s));
            CreateMap<Message, MessageDto>()
                .ForMember(x => x.SenderPhotoUrl,
                        i => i.MapFrom(k => k.Sender.Photos!.FirstOrDefault(k => k.IsMain)!.Url))
                .ForMember(x => x.RecipientPhotoUrl,
                        i => i.MapFrom(k => k.Recipient.Photos!.FirstOrDefault(k => k.IsMain)!.Url));
            CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
            CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);
        }
    }
}