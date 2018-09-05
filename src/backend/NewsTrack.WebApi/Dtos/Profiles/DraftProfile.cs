using System.Linq;
using AutoMapper;
using NewsTrack.Domain.Entities;

namespace NewsTrack.WebApi.Dtos.Profiles
{
    public class DraftProfile : Profile
    {
        public DraftProfile()
        {
            CreateMap<Draft, DraftSuggestionsDto>()
                .ForMember(m => m.Id, e => e.MapFrom(p => p.Id))
                .AfterMap((draft, dto) =>
                {
                    if (draft.Tags != null)
                    {
                        dto.Tags = draft.Tags;
                    }
                });

            CreateMap<DraftSuggestions, DraftSuggestionsDto>()
                .ForMember(m => m.Id, e => e.MapFrom(p => p.Id))
                .AfterMap((draft, dto) =>
                {
                    if (draft.Tags != null)
                    {
                        dto.Tags = draft.Tags;
                    }
                });

            CreateMap<Draft, DraftResponseDto>()
                .ForMember(m => m.Id, e => e.MapFrom(p => p.Id))
                .ForMember(m => m.Url, e => e.MapFrom(p => p.Uri));
        }
    }
}