using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using RftmAPI.Domain.Models.Tracks.Events;
using RtfmAPI.Application.Properties;

namespace RtfmAPI.Application.Events.AlbumRemovedFromTrack;

/// <summary>
/// Обработчик доменного события удаления музыкального альбома из музыкального трека.
/// </summary>
public class AlbumRemovedFromTrackDomainEventHandler : INotificationHandler<AlbumRemovedFromTrackDomainEvent>
{
    private readonly ILogger<AlbumRemovedFromTrackDomainEventHandler> _logger;

    /// <summary>
    /// Создание обработчика доменного события удаления музыкального альбома из музыкального трека.
    /// </summary>
    /// <param name="logger">Логгер.</param>
    public AlbumRemovedFromTrackDomainEventHandler(ILogger<AlbumRemovedFromTrackDomainEventHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Обработка доменного события удаления музыкального альбома из музыкального трека.
    /// </summary>
    /// <param name="notification">Событие.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task Handle(AlbumRemovedFromTrackDomainEvent notification,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("{DomainEventMessage}", Resources.DomainEventHandledSuccesfully);
        return Task.CompletedTask;
    }
}