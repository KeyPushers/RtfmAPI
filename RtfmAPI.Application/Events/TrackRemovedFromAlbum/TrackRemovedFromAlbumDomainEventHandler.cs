using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using RftmAPI.Domain.Models.Albums.Events;
using RtfmAPI.Application.Properties;

namespace RtfmAPI.Application.Events.TrackRemovedFromAlbum;

/// <summary>
/// Обработчик доменного события удаления музыкального трека из музыкального альбома.
/// </summary>
public class TrackRemovedFromAlbumDomainEventHandler : INotificationHandler<TrackRemovedFromAlbumDomainEvent>
{
    private readonly ILogger<TrackRemovedFromAlbumDomainEventHandler> _logger;

    /// <summary>
    /// Создание обработчика доменного события удаления музыкального трека из музыкального альбома.
    /// </summary>
    /// <param name="logger">Логгер.</param>
    public TrackRemovedFromAlbumDomainEventHandler(ILogger<TrackRemovedFromAlbumDomainEventHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Обработка доменного события удаления музыкального трека из музыкального альбома.
    /// </summary>
    /// <param name="notification">Доменное событие.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task Handle(TrackRemovedFromAlbumDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{DomainEventMessage}", Resources.DomainEventHandledSuccesfully);
        return Task.CompletedTask;
    }
}