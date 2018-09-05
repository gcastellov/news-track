using AutoMapper;
using NewsTrack.Browser.Dtos;

namespace NewsTrack.WebApi.Dtos.Profiles
{
    public class BrowserProfile : Profile
    {
        public BrowserProfile()
        {
            CreateMap<ResponseDto, BrowseResponseDto>()
                .ForMember(m => m.Uri, e => e.MapFrom(p => p.Uri))
                .ForMember(m => m.Paragraphs, e => e.MapFrom(p => p.Paragraphs))
                .ForMember(m => m.Pictures, e => e.MapFrom(p => p.Pictures))
                .ForMember(m => m.Titles, e => e.MapFrom(p => p.Titles));
        }
    }
}