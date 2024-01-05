using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using RftmAPI.Domain.Exceptions.AlbumExceptions;
using RftmAPI.Domain.Exceptions.GenreExceptions;
using RftmAPI.Domain.Exceptions.TrackExceptions;
using RftmAPI.Domain.Models.Albums.ValueObjects;
using RftmAPI.Domain.Models.Genres;
using RftmAPI.Domain.Models.Genres.ValueObjects;
using RftmAPI.Domain.Models.Tracks;
using RftmAPI.Domain.Models.Tracks.ValueObjects;
using RftmAPI.Domain.Primitives;
using RtfmAPI.Application.Common.Interfaces.Persistence;

namespace RtfmAPI.Application.Requests.Tracks.Commands.ModifyTrack;

/// <summary>
/// Обработчик команды изменения музыкального трека.
/// </summary>
public class ModifyTrackCommandHandler : IRequestHandler<ModifyTrackCommand, BaseResult>
{
    private readonly ITracksRepository _tracksRepository;
    private readonly IAlbumsRepository _albumsRepository;
    private readonly IGenresRepository _genresRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ModifyTrackCommandHandler> _logger;

    /// <summary>
    /// Создание обработчика команды изменения музыкального трека.
    /// </summary>
    /// <param name="tracksRepository">Репозиторий музыкальных треков.</param>
    /// <param name="albumsRepository">Репозиторий музыкальных альбомов.</param>
    /// <param name="genresRepository">Репозиторий музыкальных жанров.</param>
    /// <param name="unitOfWork">Единица работы.</param>
    /// <param name="logger">Логгер.</param>
    public ModifyTrackCommandHandler(ITracksRepository tracksRepository, IAlbumsRepository albumsRepository,
        IGenresRepository genresRepository, IUnitOfWork unitOfWork, ILogger<ModifyTrackCommandHandler> logger)
    {
        _tracksRepository = tracksRepository;
        _albumsRepository = albumsRepository;
        _genresRepository = genresRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Обработка команды музыкального трека.
    /// </summary>
    /// <param name="request">Команда изменения музыкального трека.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат операции изменения музыкального трека.</returns>
    public async Task<BaseResult> Handle(ModifyTrackCommand request, CancellationToken cancellationToken = default)
    {
        var trackId = TrackId.Create(request.TrackId);
        var track = await _tracksRepository.GetTrackByIdAsync(trackId);
        if (track is null)
        {
            var error = TrackExceptions.NotFound(trackId);
            // TODO: Добавить в ресурсы.
            _logger.LogError(error, "Не удалось изменить музыкальный трек {TrackId}", trackId.Value);
            return error;
        }

        if (request.Name is not null)
        {
            var setTrackNameResult = SetTrackName(track, request.Name);
            if (setTrackNameResult.IsFailed)
            {
                return setTrackNameResult.Error;
            }
        }

        if (request.ReleaseDate is not null)
        {
            var setTrackReleaseDateResult = SetTrackReleaseDate(track, request.ReleaseDate.Value);
            if (setTrackReleaseDateResult.IsFailed)
            {
                return setTrackReleaseDateResult.Error;
            }
        }

        if (request.AlbumId is not null)
        {
            var setAlbumResult = await SetAlbumAsync(track, request.AlbumId.Value);
            if (setAlbumResult.IsFailed)
            {
                return setAlbumResult.Error;
            }
        }

        if (request.AddingGenresIds is not null && request.AddingGenresIds.Any())
        {
            var addGenresResult = await AddGenresAsync(track, request.AddingGenresIds);
            if (addGenresResult.IsFailed)
            {
                return addGenresResult.Error;
            }
        }

        if (request.RemovingGenresIds is not null && request.RemovingGenresIds.Any())
        {
            var removeGenresResult =
                await RemoveGenresAsync(track, request.RemovingGenresIds);
            if (removeGenresResult.IsFailed)
            {
                return removeGenresResult.Error;
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return BaseResult.Success();
    }

    /// <summary>
    /// Изменение названия музыкального трека.
    /// </summary>
    /// <param name="track">Музыкальный трек.</param>
    /// <param name="name">Название музыкального трека.</param>
    private static BaseResult SetTrackName(Track track, string name)
    {
        var trackNameCreateResult = TrackName.Create(name);
        if (trackNameCreateResult.IsFailed)
        {
            return trackNameCreateResult.Error;
        }

        var setTrackNameResult = track.SetName(trackNameCreateResult.Value);
        if (setTrackNameResult.IsFailed)
        {
            return setTrackNameResult.Error;
        }

        return BaseResult.Success();
    }

    /// <summary>
    /// Изменение даты выпуска музыкального трека.
    /// </summary>
    /// <param name="track">Музыкальный трек.</param>
    /// <param name="releaseDate">Дата выпуска музыкального трека.</param>
    private static BaseResult SetTrackReleaseDate(Track track, DateTime releaseDate)
    {
        var trackReleaseDateCreateResult = TrackReleaseDate.Create(releaseDate);
        if (trackReleaseDateCreateResult.IsFailed)
        {
            return trackReleaseDateCreateResult.Error;
        }

        var trackSetReleaseDateRelease = track.SetReleaseDate(trackReleaseDateCreateResult.Value);
        if (trackSetReleaseDateRelease.IsFailed)
        {
            return trackSetReleaseDateRelease.Error;
        }

        return BaseResult.Success();
    }

    /// <summary>
    /// Изменение музыкального альбома к музыкальному треку.
    /// </summary>
    /// <param name="track">Музыкальный трек.</param>
    /// <param name="albumId">Идентификатор музыкального альбома.</param>
    private async Task<BaseResult> SetAlbumAsync(Track track, Guid albumId)
    {
        var aId = AlbumId.Create(albumId);
        var album = await _albumsRepository.GetAlbumByIdAsync(aId);
        if (album is null)
        {
            var error = AlbumExceptions.NotFound(aId);
            // TODO: Добавить в ресурсы.
            _logger.LogError(error, "Не удалось изменить музыкальный альбом {AlbumId} в музыкальном треке {TrackId}",
                aId.Value, track.Id.Value);
            return error;
        }

        var setAlbumResult = track.SetAlbum(album);
        if (setAlbumResult.IsFailed)
        {
            return setAlbumResult.Error;
        }

        var albumAddTrackResult = album.AddTracks(new[] {track});
        if (albumAddTrackResult.IsFailed)
        {
            return albumAddTrackResult.Error;
        }

        return BaseResult.Success();
    }

    /// <summary>
    /// Добавление музыкальных жанров к музыкальному треку.
    /// </summary>
    /// <param name="track">Музыкальный трек.</param>
    /// <param name="addingGenresIds">Идентификаторы добавляемых музыкальных жанров</param>
    private async Task<BaseResult> AddGenresAsync(Track track, IEnumerable<Guid> addingGenresIds)
    {
        List<Genre> addingGenres = new();
        foreach (var addingGenreId in addingGenresIds)
        {
            var genreId = GenreId.Create(addingGenreId);
            var genre = await _genresRepository.GetGenreByIdAsync(genreId);
            if (genre is null)
            {
                var error = GenreExceptions.NotFound(genreId);
                // TODO: Добавить в ресурсы.
                _logger.LogError(error,
                    "Не удалось добавить музыкальный жанр {AddingGenreId} в музыкальный трек {TrackId}",
                    addingGenreId, track.Id.Value);
                return error;
            }

            addingGenres.Add(genre);
        }

        var addGenresResult = track.AddGenres(addingGenres);
        if (addGenresResult.IsFailed)
        {
            return addGenresResult.Error;
        }

        return BaseResult.Success();
    }

    /// <summary>
    /// Удаление музыкальных жанров из музыкального трека.
    /// </summary>
    /// <param name="track">Музыкальный трек.</param>
    /// <param name="removingGenresIds">Идентификаторы удаляемых музыкальных жанров.</param>
    private async Task<BaseResult> RemoveGenresAsync(Track track, IEnumerable<Guid> removingGenresIds)
    {
        List<Genre> removingGenres = new();
        foreach (var removingGenreId in removingGenresIds)
        {
            var genreId = GenreId.Create(removingGenreId);
            var genre = await _genresRepository.GetGenreByIdAsync(genreId);
            if (genre is null)
            {
                var error = GenreExceptions.NotFound(genreId);
                // TODO: Добавить в ресурсы.
                _logger.LogError(error,
                    "Не удалось удалить музыкальный жанр {RemovingGenreId} из музыкального трека {TrackId}",
                    removingGenreId, track.Id.Value);
                return error;
            }

            removingGenres.Add(genre);
        }

        var removeGenresResult = track.RemoveGenres(removingGenres);
        if (removeGenresResult.IsFailed)
        {
            return removeGenresResult.Error;
        }

        return BaseResult.Success();
    }
}