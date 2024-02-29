using System;
using AutoMapper;
using RftmAPI.Domain.Models.TrackFiles;
using RftmAPI.Domain.Models.TrackFiles.ValueObjects;
using RtfmAPI.Infrastructure.Dao.Dao.TrackFiles;

namespace RtfmAPI.Infrastructure.Dao.MappingProfiles;

public class TrackFileMappingProfile : Profile
{
    public TrackFileMappingProfile()
    {
        CreateMap<RftmAPI.Domain.Models.TrackFiles.TrackFile, TrackFileDao>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
            .ForMember(dest => dest.Extension, opt => opt.MapFrom(src => src.Extension))
            .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.MimeType))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
            .ReverseMap();

        CreateMap<TrackFileId, Guid>()
            .ConvertUsing(src => src.Value);
        CreateMap<Guid, TrackFileId>()
            .ConvertUsing(src => TrackFileId.Create(src));

        CreateMap<TrackFileName, string>()
            .ConvertUsing(src => src.Value);
        CreateMap<string, TrackFileName>()
            .ConvertUsing(src => TrackFileName.Create(src).Value);

        CreateMap<TrackFileData, byte[]>()
            .ConvertUsing(src => src.Value);
        CreateMap<byte[], TrackFileData>()
            .ConvertUsing(src => TrackFileData.Create(src).Value);

        CreateMap<TrackFileExtension, string>()
            .ConvertUsing(src => src.Value);
        CreateMap<string, TrackFileExtension>()
            .ConvertUsing(src => TrackFileExtension.Create(src).Value);

        CreateMap<TrackFileMimeType, string>()
            .ConvertUsing(src => src.Value);
        CreateMap<string, TrackFileMimeType>()
            .ConvertUsing(src => TrackFileMimeType.Create(src).Value);

        CreateMap<TrackFileDuration, double>()
            .ConvertUsing(src => src.Value);
        CreateMap<double, TrackFileDuration>()
            .ConvertUsing(src => TrackFileDuration.Create(src).Value);
    }
}