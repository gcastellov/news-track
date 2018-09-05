using AutoMapper;
using NewsTrack.Domain.Entities;

namespace NewsTrack.WebApi.Dtos.Profiles
{
    public class NewsProfile : Profile
    {
        public NewsProfile()
        {
            CreateMap<Draft, NewsDigestDto>()
                .ForMember(m => m.Id, e => e.MapFrom(p => p.Id))
                .ForMember(m => m.Url, e => e.MapFrom(p => p.Uri))
                .ForMember(m => m.Title, e => e.MapFrom(p => p.Title))
                .ForMember(m => m.Views, e => e.MapFrom(p => p.Views))
                .ForMember(m => m.Fucks, e => e.MapFrom(p => p.Fucks));

            CreateMap<Draft, NewsDto>()
                .ForMember(m => m.Id, e => e.MapFrom(p => p.Id))
                .ForMember(m => m.Uri, e => e.MapFrom(p => p.Uri))
                .ForMember(m => m.Title, e => e.MapFrom(p => p.Title))
                .ForMember(m => m.Paragraphs, e => e.MapFrom(p => p.Paragraphs))
                .ForMember(m => m.Picture, e => e.MapFrom(p => p.Picture))
                .ForMember(m => m.CreatedAt, e => e.MapFrom(p => p.CreatedAt))
                .ForMember(m => m.Tags, e => e.MapFrom(p => p.Tags))
                .ForMember(m => m.Views, e => e.MapFrom(p => p.Views))
                .ForMember(m => m.Fucks, e => e.MapFrom(p => p.Fucks))
                .ForMember(m => m.Related, e => e.MapFrom(p => p.Related))
                .AfterMap((draft, dto) =>
                {
                    if (draft.User != null)
                    {
                        dto.CreatedBy = draft.User.Username;
                    }
                });

            CreateMap<Draft, SearchResultDto>()
                .ForMember(m => m.Id, e => e.MapFrom(p => p.Id))
                .ForMember(m => m.Content, e => e.MapFrom(p => p.Title));

            CreateMap<DraftRelationshipItem, NewsDigestDto>()
                .ForMember(m => m.Id, e => e.MapFrom(p => p.Id))
                .ForMember(m => m.Url, e => e.MapFrom(p => p.Url))
                .ForMember(m => m.Title, e => e.MapFrom(p => p.Title));

            CreateMap<Draft, NewsDigestBaseDto>()
                .ForMember(m => m.Id, e => e.MapFrom(p => p.Id))
                .ForMember(m => m.Url, e => e.MapFrom(p => p.Uri))
                .ForMember(m => m.Title, e => e.MapFrom(p => p.Title));
        }
    }
}