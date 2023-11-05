using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using RftmAPI.Domain.Models.Albums;
using RftmAPI.Domain.Models.Albums.Events;
using RftmAPI.Domain.Models.Tracks;
using RftmAPI.Domain.Primitives;
using RtfmAPI.Application.Common.Interfaces.Persistence;
using RtfmAPI.Application.Properties;

namespace RtfmAPI.Application.Events.TrackAddedToAlbum;

/// <summary>
/// Обработчик доменного события добавления музыкального трека в музыкальный альбом.
/// </summary>
public class TrackAddedToAlbumDomainEventHandler : INotificationHandler<TrackAddedToAlbumDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TrackAddedToAlbumDomainEventHandler> _logger;

    /// <summary>
    /// Создание обработчика доменного события добавления музыкального трека в музыкальный альбом.
    /// </summary>
    /// <param name="unitOfWork">Единица работы.</param>
    /// <param name="logger">Логгер.</param>
    public TrackAddedToAlbumDomainEventHandler(IUnitOfWork unitOfWork,
        ILogger<TrackAddedToAlbumDomainEventHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Обработка доменного события добавления музыкального трека в музыкальный альбом.
    /// </summary>
    /// <param name="notification">Доменное событие</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task Handle(TrackAddedToAlbumDomainEvent notification, CancellationToken cancellationToken)
    {
        var track = notification.Track;
        var album = notification.Album;

        if (!album.TrackIds.Contains(track.Id))
        {
            _logger.LogWarning("{DomainEventMessage} {Message}", Resources.DomainEventHandledWithError,
                $"Музыкальный трек [{track.Id.Value}] не был добавлен в музыкальный альбом [{album.Id.Value}].");
            return;
        }
        
        var addAlbumToTrackResult = track.AddAlbum(album);
        if (addAlbumToTrackResult.IsFailed)
        {
            var removeTrackFromAlbumResult = RemoveTrackFromAlbum(album, track);
            if (removeTrackFromAlbumResult.IsFailed)
            {
                _logger.LogWarning("{DomainEventMessage} {Message}", Resources.DomainEventHandledWithError,
                    removeTrackFromAlbumResult.Error);
                return;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("{DomainEventMessage}", Resources.DomainEventHandledSuccesfully);
    }
    
    /// <summary>
    /// Удаление музыкального трека из музыкального альбома.
    /// </summary>
    /// <param name="album">Музыкальный альбом.</param>
    /// <param name="track">Музыкальный трек.</param>
    /// <returns>Результата операции.</returns>
    private BaseResult RemoveTrackFromAlbum(Album album, Track track)
    {
        var removeTrackFromAlbumResult = album.RemoveTrack(track);
        if (removeTrackFromAlbumResult.IsFailed)
        {
            return removeTrackFromAlbumResult;
        }

        _logger.LogWarning("{DomainEventMessage} {Message}", Resources.DomainEventHandledWithError,
            $"Музыкальный трек [{track.Id.Value}] удален из музыкального альбом [{album.Id.Value}].");
        return BaseResult.Success();
    }
}