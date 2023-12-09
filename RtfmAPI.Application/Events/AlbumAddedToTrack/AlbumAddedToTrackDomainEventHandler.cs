using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using RftmAPI.Domain.Models.Tracks.Events;
using RtfmAPI.Application.Properties;

namespace RtfmAPI.Application.Events.AlbumAddedToTrack;

/// <summary>
/// Обработчик события добавления музыкального альбома к музыкальному треку.
/// </summary>
public class AlbumAddedToTrackDomainEventHandler : INotificationHandler<AlbumChangedInTrackDomainEvent>
{
    private readonly ILogger<AlbumAddedToTrackDomainEventHandler> _logger;

    /// <summary>
    /// Создание обработчика события добавления музыкального альбома к музыкальному треку.
    /// </summary>
    /// <param name="logger">Логгер.</param>
    public AlbumAddedToTrackDomainEventHandler(ILogger<AlbumAddedToTrackDomainEventHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Обработка события добавления музыкального альбома к музыкальному треку.
    /// </summary>
    /// <param name="notification">Событие <see cref="AlbumChangedInTrackDomainEvent"/>>.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task Handle(AlbumChangedInTrackDomainEvent notification, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("{DomainEventMessage}", Resources.DomainEventHandledSuccesfully);
        return Task.CompletedTask;
    }
}