using System.Collections.Generic;
using MediatR;
using RftmAPI.Domain.Models.Tracks;

namespace RtfmAPI.Application.Requests.Tracks.Queries.GetTracks;

/// <summary>
/// Запрос музыкальных треков
/// </summary>
public class GetTracksQuery : IRequest<List<Track>>
{
    
}