using System;
using AutoMapper;
using RftmAPI.Domain.Models.Genres;
using RftmAPI.Domain.Models.Genres.ValueObjects;
using RtfmAPI.Infrastructure.Dao.Dao.Genre;

namespace RtfmAPI.Infrastructure.Dao.MappingProfiles;

public class GenresMappingProfile : Profile
{
    public GenresMappingProfile()
    {
        CreateMap<Genre, GenreDao>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            // TODO: Добавить треки и бенды 
            .ReverseMap();

        CreateMap<GenreId, Guid>()
            .ConvertUsing(src => src.Value);
        CreateMap<Guid, GenreId>()
            .ConvertUsing(src => GenreId.Create(src));
        
        CreateMap<GenreName, string>()
            .ConvertUsing(src => src.Value);
        CreateMap<string, GenreName>()
            .ConvertUsing(src => GenreName.Create(src).Value);
    }
}