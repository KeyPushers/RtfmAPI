using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using RftmAPI.Domain.Models.Tracks.Events;
using RtfmAPI.Application.Common.Interfaces.Persistence;
using RtfmAPI.Application.Properties;

namespace RtfmAPI.Application.Events.AlbumRemovedFromTrack;

/// <summary>
/// Обработчик доменного события удаления музыкального альбома из музыкального трека.
/// </summary>
public class AlbumRemovedFromTrackDomainEventHandler : INotificationHandler<AlbumRemovedFromTrackDomainEvent>
{
    private readonly IAlbumsRepository _albumsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AlbumRemovedFromTrackDomainEventHandler> _logger;

    /// <summary>
    /// Создание обработчика доменного события удаления музыкального альбома из музыкального трека.
    /// </summary>
    /// <param name="albumsRepository">Репозиторий альбомов.</param>
    /// <param name="unitOfWork">Единица работы.</param>
    /// <param name="logger">Логгер.</param>
    public AlbumRemovedFromTrackDomainEventHandler(IAlbumsRepository albumsRepository, IUnitOfWork unitOfWork,
        ILogger<AlbumRemovedFromTrackDomainEventHandler> logger)
    {
        _albumsRepository = albumsRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Обработка доменного события удаления музыкального альбома из музыкального трека.
    /// </summary>
    /// <param name="notification">Событие.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task Handle(AlbumRemovedFromTrackDomainEvent notification,
        CancellationToken cancellationToken = default)
    {
        var track = notification.Track;
        if (track.AlbumId is not null)
        {
            // Прекращаем выполнение, так как альбом не был удален из музыкального трека.
            _logger.LogWarning("{DomainEventMessage} {Message}", Resources.DomainEventHandledWithError,
                $"Музыкальный альбом [{notification.AlbumId.Value}] не удален из музыкального трека [{track.Id.Value}].");
            return;
        }

        var album = await _albumsRepository.GetAlbumByIdAsync(notification.AlbumId);
        if (album is null || !album.TrackIds.Contains(track.Id))
        {
            // Если музыкальный альбом не существует или в музыкальном альбом нет такого музыкального трека, то прекращаем выполнение.
            _logger.LogInformation("{DomainEventMessage}", Resources.DomainEventHandledSuccesfully);
            return;
        }

        // Удаляем музыкальный трек из музыкального альбома.
        var removeTrackFromAlbumResult = album.RemoveTrack(track);
        if (removeTrackFromAlbumResult.IsFailed)
        {
            // Если не получилось удалить альбом из трека, то восстанавливаем альбом в треке.
            var addAlbumResult = track.AddAlbum(album);
            if (addAlbumResult.IsFailed)
            {
                // Прекращаем выполнение, если операция восстановления альбома в трека провалилась.
                _logger.LogWarning("{DomainEventMessage} {Message}", Resources.DomainEventHandledWithError,
                    addAlbumResult.Error);
                return;
            }
            
            _logger.LogWarning("{DomainEventMessage} {Message}", Resources.DomainEventHandledWithError,
                removeTrackFromAlbumResult.Error);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("{DomainEventMessage}", Resources.DomainEventHandledSuccesfully);
    }
}