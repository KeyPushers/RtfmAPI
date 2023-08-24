using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RftmAPI.Domain.Aggregates.Tracks;

namespace RtfmAPI.Application.Requests.Tracks.Queries.GetTracks;

/// <summary>
/// Обработчик запроса музыкальных треков
/// </summary>
public class GetTracksQueryHandler : IRequestHandler<GetTracksQuery, List<Track>>
{
    private readonly ITrackRepository _trackRepository;

    /// <summary>
    /// Обработчик запроса музыкальных треков
    /// </summary>
    /// <param name="trackRepository">Репозиторий треков</param>
    public GetTracksQueryHandler(ITrackRepository trackRepository)
    {
        _trackRepository = trackRepository;
    }
    
    /// <summary>
    /// Обработка запроса музыкальных треков
    /// </summary>
    /// <param name="request">Запрос музыкальных треков</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Музыкальные треки</returns>
    public Task<List<Track>> Handle(GetTracksQuery request, CancellationToken cancellationToken = default)
    {
        return _trackRepository.GetTracksAsync();
    }
}