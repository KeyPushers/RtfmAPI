using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using RftmAPI.Domain.Models.Albums.Events;
using RtfmAPI.Application.Properties;

namespace RtfmAPI.Application.Events.TrackAddedToAlbum;

/// <summary>
/// Обработчик доменного события добавления музыкального трека в музыкальный альбом.
/// </summary>
public class TrackAddedToAlbumDomainEventHandler : INotificationHandler<TrackAddedToAlbumDomainEvent>
{
    private readonly ILogger<TrackAddedToAlbumDomainEventHandler> _logger;

    /// <summary>
    /// Создание обработчика доменного события добавления музыкального трека в музыкальный альбом.
    /// </summary>
    /// <param name="logger">Логгер.</param>
    public TrackAddedToAlbumDomainEventHandler(ILogger<TrackAddedToAlbumDomainEventHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Обработка доменного события добавления музыкального трека в музыкальный альбом.
    /// </summary>
    /// <param name="notification">Доменное событие</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task Handle(TrackAddedToAlbumDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{DomainEventMessage}", Resources.DomainEventHandledSuccesfully);
        return Task.CompletedTask;
    }
}