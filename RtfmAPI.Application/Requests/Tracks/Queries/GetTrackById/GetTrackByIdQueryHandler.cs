using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RftmAPI.Domain.Aggregates.Tracks;

namespace RtfmAPI.Application.Requests.Tracks.Queries.GetTrackById;

/// <summary>
/// Обработчик запроса музыкального трека по идентификатору
/// </summary>
public class GetTrackByIdQueryHandler : IRequestHandler<GetTrackByIdQuery, Track?>
{
    private readonly ITrackRepository _trackRepository;

    /// <summary>
    /// Обработчик запроса музыкального трека по идентификатору
    /// </summary>
    /// <param name="trackRepository">Репозиторий музыкальных треков</param>
    public GetTrackByIdQueryHandler(ITrackRepository trackRepository)
    {
        _trackRepository = trackRepository;
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
        return _trackRepository.GetTrackByIdAsync(request.Id);
    }
}