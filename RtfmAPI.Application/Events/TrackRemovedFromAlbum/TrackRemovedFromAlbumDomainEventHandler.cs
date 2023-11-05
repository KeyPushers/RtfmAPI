using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using RftmAPI.Domain.Models.Albums.Events;
using RtfmAPI.Application.Common.Interfaces.Persistence;
using RtfmAPI.Application.Properties;

namespace RtfmAPI.Application.Events.TrackRemovedFromAlbum;

/// <summary>
/// Обработчик доменного события удаления музыкального трека из музыкального альбома.
/// </summary>
public class TrackRemovedFromAlbumDomainEventHandler : INotificationHandler<TrackRemovedFromAlbumDomainEvent>
{
    private readonly ITracksRepository _tracksRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TrackRemovedFromAlbumDomainEventHandler> _logger;

    /// <summary>
    /// Создание обработчика доменного события удаления музыкального трека из музыкального альбома.
    /// </summary>
    /// <param name="tracksRepository">Репозиторий музыкальных треков</param>
    /// <param name="unitOfWork">Единица работы.</param>
    /// <param name="logger">Логгер.</param>
    public TrackRemovedFromAlbumDomainEventHandler(ITracksRepository tracksRepository, IUnitOfWork unitOfWork,
        ILogger<TrackRemovedFromAlbumDomainEventHandler> logger)
    {
        _tracksRepository = tracksRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Обработка доменного события удаления музыкального трека из музыкального альбома.
    /// </summary>
    /// <param name="notification">Доменное событие.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task Handle(TrackRemovedFromAlbumDomainEvent notification, CancellationToken cancellationToken)
    {
        var album = notification.Album;
        if (album.TrackIds.Contains(notification.TrackId))
        {
            // Прекращаем выполнение, если музыкальный трек не удален из музыкального альбома.
            _logger.LogWarning("{DomainEventMessage} {Message}", Resources.DomainEventHandledWithError,
                $"Музыкальный трек [{notification.TrackId.Value}] не удален из музыкального альбома [{album.Id.Value}].");
            return;
        }

        var track = await _tracksRepository.GetTrackByIdAsync(notification.TrackId);
        if (track?.AlbumId is null)
        {
            // Если музыкальный трек не найден или музыкальный альбом отсутствует в музыкальном треке, то прекращаем выполнение.
            _logger.LogInformation("{DomainEventMessage}", Resources.DomainEventHandledSuccesfully);
            return;
        }

        // Удаляем музыкальный альбом из музыкального трека.
        var removeAlbumFromTrackResult = track.RemoveAlbum();
        if (removeAlbumFromTrackResult.IsFailed)
        {
            // При неудачной попытке удалить альбом из трека восстанавливаем трек в альбоме.
            var addTrackToAlbumResult = album.AddTrack(track);
            if (addTrackToAlbumResult.IsFailed)
            {
                // Прекращаем выполнение, если операция восстановления трека в альбоме провалилась.
                _logger.LogWarning("{DomainEventMessage} {Message}", Resources.DomainEventHandledWithError,
                    addTrackToAlbumResult.Error);
                return;
            }

            _logger.LogWarning("{DomainEventMessage} {Message}", Resources.DomainEventHandledWithError,
                removeAlbumFromTrackResult.Error);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("{DomainEventMessage}", Resources.DomainEventHandledSuccesfully);
    }
}