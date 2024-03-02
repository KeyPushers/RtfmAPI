using System;
using AutoMapper;
using RftmAPI.Domain.Models.Tracks;
using RftmAPI.Domain.Models.Tracks.ValueObjects;
using RtfmAPI.Infrastructure.Dao.Dao.Tracks;

namespace RtfmAPI.Infrastructure.Dao.MappingProfiles;

public class TracksMappingProfile : Profile
{
    public TracksMappingProfile()
    {
        CreateMap<Track, TrackDao>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
            .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.ReleaseDate))
            .ForMember(dest => dest.TrackFileId, opt => opt.MapFrom(src => src.TrackFileId))
            .ForMember(dest => dest.GenreIds,
                opt => opt.MapFrom(src => src.GenreIds))
            .ReverseMap();

        CreateMap<TrackId, Guid>()
            .ConvertUsing(src => src.Value);
        CreateMap<Guid, TrackId>()
            .ConvertUsing(src => TrackId.Create(src));

        CreateMap<TrackName, string>()
            .ConvertUsing(src => src.Value.ToString());
        CreateMap<string, TrackName>()
            .ConvertUsing(src => TrackName.Create(src).Value);

        CreateMap<TrackReleaseDate, DateTime>()
            .ConvertUsing(src => src.Value);
        CreateMap<DateTime, TrackReleaseDate>()
            .ConvertUsing(src => TrackReleaseDate.Create(src).Value);

        CreateMap<TrackDuration, double>()
            .ConvertUsing(src => src.Value);
        CreateMap<double, TrackDuration>()
            .ConvertUsing(src => TrackDuration.Create(src).Value);
    }
}