using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using RftmAPI.Domain.Models.Tracks.Events;
using RftmAPI.Domain.Models.Tracks.ValueObjects;
using RtfmAPI.Application.Common.Interfaces.Persistence;

namespace RtfmAPI.Application.Events.TrackCreated;

/// <summary>
/// Обработчик события создания музыкального трека.
/// </summary>
public class TrackCreatedDomainEventHandler : INotificationHandler<TrackCreatedDomainEvent>
{
    private readonly ILogger<TrackCreatedDomainEventHandler> _logger;
    private readonly ITracksRepository _repository;

    /// <summary>
    /// Создание обработчика события создания музыкального трека.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="repository">Репозиторий музыкальных треков.</param>
    public TrackCreatedDomainEventHandler(ILogger<TrackCreatedDomainEventHandler> logger, ITracksRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }
    
    /// <summary>
    /// Обработка события создания музыкального трека.
    /// </summary>
    /// <param name="notification">Событие</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public async Task Handle(TrackCreatedDomainEvent notification, CancellationToken cancellationToken = default)
    {
        _logger.LogTrace("Обработка события создания музыкального трека: {IdValue}", notification.Track.Id.Value);

        var track = await _repository.GetTrackByIdAsync((TrackId) notification.Track.Id);
        
        return;
    }
}