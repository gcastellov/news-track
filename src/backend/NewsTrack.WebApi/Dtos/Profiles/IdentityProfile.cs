﻿using AutoMapper;

namespace NewsTrack.WebApi.Dtos.Profiles
{
    public class IdentityProfile : Profile
    {
        public IdentityProfile()
        {
            CreateMap<Identity.Identity, IdentityDto>()
                .ForMember(m => m.Username, e => e.MapFrom(p => p.Username))
                .ForMember(m => m.Email, e => e.MapFrom(p => p.Email))
                .ForMember(m => m.IsEnabled, e => e.MapFrom(p => p.IsEnabled))
                .ForMember(m => m.IdType, e => e.MapFrom(p => p.IdType))
                .ForMember(m => m.CreatedAt, e => e.MapFrom(p => p.CreatedAt))
                .ForMember(m => m.LastAccessAt, e => e.MapFrom(p => p.LastAccessAt))
                .ForMember(m => m.LastAccessFailureAt, e => e.MapFrom(p => p.LastAccessFailureAt));
        }
    }
}