using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using RftmAPI.Domain.Models.Tracks;
using RftmAPI.Domain.Models.Tracks.Events;
using RftmAPI.Domain.Primitives;
using RtfmAPI.Application.Common.Interfaces.Persistence;
using RtfmAPI.Application.Properties;

namespace RtfmAPI.Application.Events.AlbumAddedToTrack;

/// <summary>
/// Обработчик события добавления музыкального альбома к музыкальному треку.
/// </summary>
public class AlbumAddedToTrackDomainEventHandler : INotificationHandler<AlbumAddedToTrackDomainEvent>
{
    private readonly IAlbumsRepository _albumsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AlbumAddedToTrackDomainEventHandler> _logger;

    /// <summary>
    /// Создание обработчика события добавления музыкального альбома к музыкальному треку.
    /// </summary>
    /// <param name="albumsRepository">Репозиторий музыкальных альбомов.
    /// </param>
    /// <param name="unitOfWork">Единица работы.</param>
    /// <param name="logger">Логгер.</param>
    public AlbumAddedToTrackDomainEventHandler(IAlbumsRepository albumsRepository, IUnitOfWork unitOfWork,
        ILogger<AlbumAddedToTrackDomainEventHandler> logger)
    {
        _albumsRepository = albumsRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Обработка события добавления музыкального альбома к музыкальному треку.
    /// </summary>
    /// <param name="notification">Событие <see cref="AlbumAddedToTrackDomainEvent"/>>.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task Handle(AlbumAddedToTrackDomainEvent notification, CancellationToken cancellationToken = default)
    {
        var track = notification.Track;
        var album = notification.Album;

        if (track.AlbumId is null)
        {
            _logger.LogWarning("{DomainEventMessage} {Message}", Resources.DomainEventHandledWithError,
                $"Музыкальный альбом [{album.Id.Value}] не был добавлен в музыкальный трек [{track.Id.Value}].");
            return;
        }

        if (track.AlbumId != album.Id)
        {
            var removeAlbumFromTrackResult = RemoveAlbumFromTrack(track);
            if (removeAlbumFromTrackResult.IsFailed)
            {
                _logger.LogWarning("{DomainEventMessage} {Message}", Resources.DomainEventHandledWithError,
                    removeAlbumFromTrackResult.Error);
                return;
            }
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return;
        }

        var addTrackToAlbumResult = album.AddTrack(track);
        if (addTrackToAlbumResult.IsFailed)
        {
            var removeAlbumFromTrackResult = RemoveAlbumFromTrack(track);
            if (removeAlbumFromTrackResult.IsFailed)
            {
                _logger.LogWarning("{DomainEventMessage} {Message}", Resources.DomainEventHandledWithError,
                    removeAlbumFromTrackResult.Error);
                return;    
            }
            
            _logger.LogWarning("{DomainEventMessage} {Message}", Resources.DomainEventHandledWithError,
                addTrackToAlbumResult.Error);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("{DomainEventMessage}", Resources.DomainEventHandledSuccesfully);
    }
    
    /// <summary>
    /// Удаление музыкального альбома из музыкального трека.
    /// </summary>
    /// <param name="track">Музыкальный трек.</param>
    private BaseResult RemoveAlbumFromTrack(Track track)
    {
        var albumId = track.AlbumId;
        
        var removingAlbumFromTrackResult = track.RemoveAlbum();
        if (removingAlbumFromTrackResult.IsFailed)
        {
            return removingAlbumFromTrackResult;
        }

        _logger.LogWarning("{DomainEventMessage} {Message}", Resources.DomainEventHandledWithError,
            $"Музыкальный альбом [{albumId?.Value}] удален из музыкального трека [{track.Id.Value}].");
        return BaseResult.Success();
    }
}