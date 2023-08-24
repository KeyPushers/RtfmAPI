using System.Collections.Generic;
using MediatR;
using RftmAPI.Domain.Aggregates.Tracks;

namespace RtfmAPI.Application.Requests.Tracks.Queries.GetTracks;

/// <summary>
/// Запрос музыкальных треков
/// </summary>
public class GetTracksQuery : IRequest<List<Track>>
{
    
}