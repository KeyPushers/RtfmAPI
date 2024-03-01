using System;
using AutoMapper;
using RftmAPI.Domain.Models.Albums;
using RftmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Infrastructure.Dao.Dao.Albums;
using RtfmAPI.Infrastructure.Dao.Dao.BandAlbum;

namespace RtfmAPI.Infrastructure.Dao.MappingProfiles;

public class AlbumsMappingProfile : Profile
{
    public AlbumsMappingProfile()
    {
        CreateMap<Album, AlbumDao>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.ReleaseDate))
            .ForMember(dest => dest.TrackIds, opt => opt.MapFrom(src => src.TrackIds))
            .ForMember(dest => dest.BandIds, opt => opt.MapFrom(src => src.BandIds))
            // .ForMember(dest => dest.BandAlbums, opt => opt.MapFrom(src => src.BandIds))
            .ReverseMap();

        CreateMap<AlbumId, Guid>()
            .ConvertUsing(src => src.Value);
        CreateMap<Guid, AlbumId>()
            .ConvertUsing(src => AlbumId.Create(src));
        
        CreateMap<AlbumName, string>()
            .ConvertUsing(src => src.Value);
        CreateMap<string, AlbumName>()
            .ConvertUsing(src => AlbumName.Create(src).Value);
        
        CreateMap<AlbumReleaseDate, DateTime>()
            .ConvertUsing(src => src.Value);
        CreateMap<DateTime, AlbumReleaseDate>()
            .ConvertUsing(src => AlbumReleaseDate.Create(src).Value);
        
        CreateMap<AlbumId, BandAlbumDao>()
            .ConvertUsing(AlbumId2BandAlbumDao);
        CreateMap<BandAlbumDao, AlbumId>()
            .ConvertUsing(src => AlbumId.Create(src.AlbumId));
    }

    private BandAlbumDao AlbumId2BandAlbumDao(AlbumId albumId, BandAlbumDao bandAlbum, ResolutionContext context)
    {
        return new BandAlbumDao
        {
            BandId = default,
            Band = null,
            AlbumId = albumId.Value,
            Album = null
        };
    }
}