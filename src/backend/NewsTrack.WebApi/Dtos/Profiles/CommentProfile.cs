using AutoMapper;
using NewsTrack.Domain.Entities;

namespace NewsTrack.WebApi.Dtos.Profiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentDto>()
                .ForMember(m => m.Id, e => e.MapFrom(p => p.Id))
                .ForMember(m => m.CreatedAt, e => e.MapFrom(p => p.CreatedAt))
                .ForMember(m => m.CreatedBy, e => e.MapFrom(p => p.CreatedBy.Username))
                .ForMember(m => m.Content, e => e.MapFrom(p => p.Content))
                .ForMember(m => m.DraftId, e => e.MapFrom(p => p.DraftId))
                .ForMember(m => m.ReplyingTo, e => e.MapFrom(p => p.ReplyingTo))
                .ForMember(m => m.Likes, e => e.MapFrom(p => p.Likes));
        }
    }
}
