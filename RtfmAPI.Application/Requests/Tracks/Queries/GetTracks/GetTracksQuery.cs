using MediatR;
using RtfmAPI.Application.Requests.Tracks.Queries.GetTracks.Dtos;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Application.Requests.Tracks.Queries.GetTracks;

/// <summary>
/// Запрос музыкальных треков
/// </summary>
public class GetTracksQuery : IRequest<Result<TrackItems>>
{
    
}