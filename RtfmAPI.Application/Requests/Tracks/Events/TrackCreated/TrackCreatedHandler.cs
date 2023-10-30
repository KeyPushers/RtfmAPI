using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using TrackCreatedEvent = RftmAPI.Domain.Models.Tracks.Events.TrackCreated;

namespace RtfmAPI.Application.Requests.Tracks.Events.TrackCreated;

/// <summary>
/// Обработчик события создания музыкального трека.
/// </summary>
public class TrackCreatedHandler : INotificationHandler<TrackCreatedEvent>
{
    private readonly ILogger<TrackCreatedHandler> _logger;

    /// <summary>
    /// Создание обработчика события создания музыкального трека.
    /// </summary>
    /// <param name="logger"></param>
    public TrackCreatedHandler(ILogger<TrackCreatedHandler> logger)
    {
        _logger = logger;
    }
    
    /// <summary>
    /// Обработка события создания музыкального трека.
    /// </summary>
    /// <param name="notification">Событие</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public Task Handle(TrackCreatedEvent notification, CancellationToken cancellationToken = default)
    {
        _logger.LogTrace("Обработка события создания музыкального трека: {IdValue}", notification.Track.Id.Value);
        
        return Task.CompletedTask;
    }
}