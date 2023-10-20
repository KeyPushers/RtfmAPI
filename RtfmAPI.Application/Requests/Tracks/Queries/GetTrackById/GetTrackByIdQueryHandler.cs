using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RftmAPI.Domain.Models.Tracks;
using RftmAPI.Domain.Models.Tracks.Repository;

namespace RtfmAPI.Application.Requests.Tracks.Queries.GetTrackById;

/// <summary>
/// Обработчик запроса музыкального трека по идентификатору
/// </summary>
public class GetTrackByIdQueryHandler : IRequestHandler<GetTrackByIdQuery, Track?>
{
    private readonly ITracksRepository _tracksRepository;

    /// <summary>
    /// Обработчик запроса музыкального трека по идентификатору
    /// </summary>
    /// <param name="tracksRepository">Репозиторий музыкальных треков</param>
    public GetTrackByIdQueryHandler(ITracksRepository tracksRepository)
    {
        _tracksRepository = tracksRepository;
    }
    
    /// <summary>
    /// Обработка запроса музыкального трека по идентификатору
    /// </summary>
    /// <param name="request">Запрос музыкального трека по идентификатору</param>
    /// <param name="cancellationToken">Токен отменеы</param>
    /// <returns>Музыкальный трек</returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<Track?> Handle(GetTrackByIdQuery request, CancellationToken cancellationToken = default)
    {
        return _tracksRepository.GetTrackByIdAsync(request.Id);
    }
}