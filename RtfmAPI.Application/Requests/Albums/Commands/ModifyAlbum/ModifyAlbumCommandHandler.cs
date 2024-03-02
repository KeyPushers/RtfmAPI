using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using RftmAPI.Domain.Exceptions.AlbumExceptions;
using RftmAPI.Domain.Exceptions.TrackExceptions;
using RftmAPI.Domain.Models.Albums;
using RftmAPI.Domain.Models.Albums.ValueObjects;
using RftmAPI.Domain.Models.Tracks;
using RftmAPI.Domain.Models.Tracks.ValueObjects;
using RftmAPI.Domain.Primitives;
using RtfmAPI.Application.Common.Interfaces.Persistence;

namespace RtfmAPI.Application.Requests.Albums.Commands.ModifyAlbum;

/// <summary>
/// Обработчик команды изменения музыкального альбома.
/// </summary>
public class ModifyAlbumCommandHandler : IRequestHandler<ModifyAlbumCommand, BaseResult>
{
    private readonly IAlbumsRepository _albumsRepository;
    private readonly ITracksRepository _tracksRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ModifyAlbumCommandHandler> _logger;

    /// <summary>
    /// Создание обработчика команды изменения музыкального альбома.
    /// </summary>
    /// <param name="albumsRepository">Репозиторий музыкального альбома.</param>
    /// <param name="tracksRepository">Репозиторий музыкального трека.</param>
    /// <param name="unitOfWork">Единица работы.</param>
    /// <param name="logger">Логгер.</param>
    public ModifyAlbumCommandHandler(IAlbumsRepository albumsRepository, ITracksRepository tracksRepository,
        IUnitOfWork unitOfWork, ILogger<ModifyAlbumCommandHandler> logger)
    {
        _albumsRepository = albumsRepository;
        _tracksRepository = tracksRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Обработка команды изменения музыкального альбома.
    /// </summary>
    /// <param name="request">Команда изменения музыкального альбома.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат выполнения команды.</returns>
    public async Task<BaseResult> Handle(ModifyAlbumCommand request, CancellationToken cancellationToken = default)
    {
        var albumId = AlbumId.Create(request.AlbumId);
        var album = await _albumsRepository.GetAlbumByIdAsync(albumId);
        if (album is null)
        {
            var error = AlbumExceptions.NotFound(albumId);
            // TODO: Добавить в ресурсы.
            _logger.LogError(error, "Не удалось изменить музыкальный альбом {AlbumId}", albumId.Value);
            return error;
        }

        if (request.Name is not null)
        {
            var setAlbumNameResult = SetAlbumName(album, request.Name);
            if (setAlbumNameResult.IsFailed)
            {
                return setAlbumNameResult.Error;
            }
        }

        if (request.ReleaseDate is not null)
        {
            var setAlbumReleaseDateResult = SetAlbumReleaseDate(album, request.ReleaseDate.Value);
            if (setAlbumReleaseDateResult.IsFailed)
            {
                return setAlbumReleaseDateResult.Error;
            }
        }

        if (request.AddingTracksIds is not null && request.AddingTracksIds.Any())
        {
            var addTracksResult = await AddTracksAsync(album, request.AddingTracksIds);
            if (addTracksResult.IsFailed)
            {
                return addTracksResult.Error;
            }
        }

        if (request.RemovingTracksIds is not null && request.RemovingTracksIds.Any())
        {
            var removeTracksResult = await RemoveTracksAsync(album, request.RemovingTracksIds);
            if (removeTracksResult.IsFailed)
            {
                return removeTracksResult.Error;
            }
        }

        await _albumsRepository.UpdateAsync(album);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return BaseResult.Success();
    }

    /// <summary>
    /// Установка названия музыкального альбома.
    /// </summary>
    /// <param name="album">Музыкальный альбом.</param>
    /// <param name="name">Название музыкального альбома.</param>
    private static BaseResult SetAlbumName(Album album, string name)
    {
        var albumNameCreateResult = AlbumName.Create(name);
        if (albumNameCreateResult.IsFailed)
        {
            return albumNameCreateResult.Error;
        }

        var setAlbumNameResult = album.SetName(albumNameCreateResult.Value);
        return setAlbumNameResult.IsFailed ? setAlbumNameResult.Error : BaseResult.Success();
    }

    /// <summary>
    /// Установка даты выпуска музыкального альбома.
    /// </summary>
    /// <param name="album">Музыкальный альбом.</param>
    /// <param name="releaseDate">Дата выпуска музыкального альбома.</param>
    private static BaseResult SetAlbumReleaseDate(Album album, DateTime releaseDate)
    {
        var albumReleaseDateCreateResult = AlbumReleaseDate.Create(releaseDate);
        if (albumReleaseDateCreateResult.IsFailed)
        {
            return albumReleaseDateCreateResult.Error;
        }

        var albumSetReleaseDateRelease = album.SetReleaseDate(albumReleaseDateCreateResult.Value);
        if (albumSetReleaseDateRelease.IsFailed)
        {
            return albumSetReleaseDateRelease.Error;
        }

        return BaseResult.Success();
    }

    /// <summary>
    /// Добавление музыкальных треков в музыкальный альбома.
    /// </summary>
    /// <param name="album">Музыкальный альбом.</param>
    /// <param name="addingTracksIds">Идентификаторы, добавляемых музыкальных треков.</param>
    private async Task<BaseResult> AddTracksAsync(Album album, IEnumerable<Guid> addingTracksIds)
    {
        List<Track> addingTracks = new();
        foreach (var addingTrackId in addingTracksIds)
        {
            var trackId = TrackId.Create(addingTrackId);
            var track = await _tracksRepository.GetTrackByIdAsync(trackId);
            if (track is null)
            {
                var error = TrackExceptions.NotFound(trackId);
                _logger.LogError(error,
                    "Не удалось добавить музыкальный трек {AddingTrackId} в музыкальный альбом {AlbumId}",
                    trackId.Value, album.Id.Value);
                return error;
            }

            addingTracks.Add(track);
        }

        var addTracksResult = album.AddTracks(addingTracks);
        return addTracksResult.IsFailed ? addTracksResult.Error : BaseResult.Success();
    }

    /// <summary>
    /// Удаление музыкальных треков из музыкального альбома.
    /// </summary>
    /// <param name="album">Музыкальный альбом.</param>
    /// <param name="removingTracksIds">Идентификаторы, удаляемых музыкальных треков.</param>
    private async Task<BaseResult> RemoveTracksAsync(Album album, IEnumerable<Guid> removingTracksIds)
    {
        List<Track> removingTracks = new();
        foreach (var removingTrackId in removingTracksIds)
        {
            var trackId = TrackId.Create(removingTrackId);
            var track = await _tracksRepository.GetTrackByIdAsync(trackId);
            if (track is null)
            {
                var error = TrackExceptions.NotFound(trackId);
                _logger.LogError(error,
                    "Не удалось удалить музыкальный трек {RemovingTrackId} из музыкального альбома {AlbumId}",
                    trackId.Value, album.Id.Value);
                return error;
            }

            removingTracks.Add(track);
        }

        var removeTracksResult = album.RemoveTracks(removingTracks);
        return removeTracksResult.IsFailed ? removeTracksResult.Error : BaseResult.Success();
    }
}