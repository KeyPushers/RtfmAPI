using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RftmAPI.Domain.Models.Tracks;
using RtfmAPI.Application.Common.Interfaces.Persistence;

namespace RtfmAPI.Application.Requests.Tracks.Queries.GetTracks;

/// <summary>
/// Обработчик запроса музыкальных треков
/// </summary>
public class GetTracksQueryHandler : IRequestHandler<GetTracksQuery, List<Track>>
{
    private readonly ITracksRepository _tracksRepository;

    /// <summary>
    /// Обработчик запроса музыкальных треков
    /// </summary>
    /// <param name="tracksRepository">Репозиторий треков</param>
    public GetTracksQueryHandler(ITracksRepository tracksRepository)
    {
        _tracksRepository = tracksRepository;
    }

    /// <summary>
    /// Обработка запроса музыкальных треков
    /// </summary>
    /// <param name="request">Запрос музыкальных треков</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Музыкальные треки</returns>
    public async Task<List<Track>> Handle(GetTracksQuery request, CancellationToken cancellationToken = default)
    {
        var result = await _tracksRepository.GetTracksAsync().ConfigureAwait(false);
        return result;
    }
}