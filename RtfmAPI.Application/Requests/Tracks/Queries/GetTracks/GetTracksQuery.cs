using MediatR;
using RftmAPI.Domain.Primitives;
using RtfmAPI.Application.Requests.Tracks.Queries.GetTracks.Dtos;

namespace RtfmAPI.Application.Requests.Tracks.Queries.GetTracks;

/// <summary>
/// Запрос музыкальных треков
/// </summary>
public class GetTracksQuery : IRequest<Result<TrackItems>>
{
    
}