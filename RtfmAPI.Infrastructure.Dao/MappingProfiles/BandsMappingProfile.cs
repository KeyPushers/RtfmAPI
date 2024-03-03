// using System;
// using AutoMapper;
// using RftmAPI.Domain.Models.Bands;
// using RftmAPI.Domain.Models.Bands.ValueObjects;
// using RtfmAPI.Infrastructure.Dao.Dao.Bands;
//
// namespace RtfmAPI.Infrastructure.Dao.MappingProfiles;
//
// public class BandsMappingProfile : Profile
// {
//     public BandsMappingProfile()
//     {
//         CreateMap<Band, BandDao>()
//             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
//             .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
//             .ForMember(dest => dest.AlbumIds, opt => opt.MapFrom(src => src.AlbumIds))
//             .ForMember(dest => dest.GenreIds, opt => opt.MapFrom(src => src.GenreIds))
//             .ReverseMap();
//
//         CreateMap<BandId, Guid>()
//             .ConvertUsing(src => src.Value);
//         CreateMap<Guid, BandId>()
//             .ConvertUsing(src => BandId.Create(src));
//
//         CreateMap<BandName, string>()
//             .ConvertUsing(src => src.Value);
//         CreateMap<string, BandName>()
//             .ConvertUsing(src => BandName.Create(src).Value);
//     }
//     
// }