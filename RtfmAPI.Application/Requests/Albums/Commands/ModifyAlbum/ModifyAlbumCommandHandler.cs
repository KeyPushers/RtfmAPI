using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using MediatR;
using RtfmAPI.Application.Interfaces.Persistence.Commands;
using RtfmAPI.Application.Interfaces.Persistence.Queries;
using RtfmAPI.Domain.Models.Albums;
using RtfmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Domain.Models.Tracks.ValueObjects;

namespace RtfmAPI.Application.Requests.Albums.Commands.ModifyAlbum;

/// <summary>
/// Обработчик команды изменения музыкального альбома.
/// </summary>
public class ModifyAlbumCommandHandler : IRequestHandler<ModifyAlbumCommand, Result>
{
    private readonly IAlbumsCommandsRepository _albumsCommandsRepository;
    private readonly IAlbumsQueriesRepository _albumsQueriesRepository;
    private readonly ITracksQueriesRepository _tracksQueriesRepository;

    /// <summary>
    /// Создание обработчика команды изменения музыкального альбома.
    /// </summary>
    /// <param name="albumsCommandsRepository">Репозиторий команд музыкального альбома.</param>
    /// <param name="albumsQueriesRepository">Репозиторий запросов музыкального альбома.</param>
    /// <param name="tracksQueriesRepository">Репозиторий запросов музыкальных треков.</param>
    public ModifyAlbumCommandHandler(IAlbumsCommandsRepository albumsCommandsRepository,
        IAlbumsQueriesRepository albumsQueriesRepository, ITracksQueriesRepository tracksQueriesRepository)
    {
        _albumsCommandsRepository = albumsCommandsRepository;
        _albumsQueriesRepository = albumsQueriesRepository;
        _tracksQueriesRepository = tracksQueriesRepository;
    }

    /// <summary>
    /// Обработка команды изменения музыкального альбома.
    /// </summary>
    /// <param name="request">Команда изменения музыкального альбома.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат выполнения команды.</returns>
    public async Task<Result> Handle(ModifyAlbumCommand request, CancellationToken cancellationToken = default)
    {
        var albumId = AlbumId.Create(request.AlbumId);
        var getAlbumResult = await _albumsQueriesRepository.GetAlbumByIdAsync(albumId);
        if (getAlbumResult.IsFailed)
        {
            return getAlbumResult.ToResult();
        }

        var album = getAlbumResult.ValueOrDefault;

        if (request.Name is not null)
        {
            var setAlbumNameResult = SetAlbumName(album, request.Name);
            if (setAlbumNameResult.IsFailed)
            {
                return setAlbumNameResult;
            }
        }

        if (request.ReleaseDate is not null)
        {
            var setAlbumReleaseDateResult = SetAlbumReleaseDate(album, request.ReleaseDate.Value);
            if (setAlbumReleaseDateResult.IsFailed)
            {
                return setAlbumReleaseDateResult;
            }
        }

        if (request.AddingTracksIds is not null && request.AddingTracksIds.Any())
        {
            var addTracksResult = await AddTracksAsync(album, request.AddingTracksIds);
            if (addTracksResult.IsFailed)
            {
                return addTracksResult;
            }
        }

        if (request.RemovingTracksIds is not null && request.RemovingTracksIds.Any())
        {
            var removeTracksResult = RemoveTracks(album, request.RemovingTracksIds);
            if (removeTracksResult.IsFailed)
            {
                return removeTracksResult;
            }
        }

        await _albumsCommandsRepository.CommitChangesAsync(album, cancellationToken);
        return Result.Ok();
    }

    /// <summary>
    /// Установка названия музыкального альбома.
    /// </summary>
    /// <param name="album">Музыкальный альбом.</param>
    /// <param name="name">Название музыкального альбома.</param>
    private static Result SetAlbumName(Album album, string name)
    {
        var albumNameCreateResult = AlbumName.Create(name);
        if (albumNameCreateResult.IsFailed)
        {
            return albumNameCreateResult.ToResult();
        }

        var setAlbumNameResult = album.SetName(albumNameCreateResult.Value);
        if (setAlbumNameResult.IsFailed)
        {
            return setAlbumNameResult;
        }

        return Result.Ok();
    }

    /// <summary>
    /// Установка даты выпуска музыкального альбома.
    /// </summary>
    /// <param name="album">Музыкальный альбом.</param>
    /// <param name="releaseDate">Дата выпуска музыкального альбома.</param>
    private static Result SetAlbumReleaseDate(Album album, DateTime releaseDate)
    {
        var albumReleaseDateCreateResult = AlbumReleaseDate.Create(releaseDate);
        if (albumReleaseDateCreateResult.IsFailed)
        {
            return albumReleaseDateCreateResult.ToResult();
        }

        var albumSetReleaseDateRelease = album.SetReleaseDate(albumReleaseDateCreateResult.Value);
        if (albumSetReleaseDateRelease.IsFailed)
        {
            return albumSetReleaseDateRelease;
        }

        return Result.Ok();
    }

    /// <summary>
    /// Добавление музыкальных треков в музыкальный альбома.
    /// </summary>
    /// <param name="album">Музыкальный альбом.</param>
    /// <param name="ids">Идентификаторы, добавляемых музыкальных треков.</param>
    private async Task<Result> AddTracksAsync(Album album, IEnumerable<Guid> ids)
    {
        var addingTrackIds = ids.Select(TrackId.Create).ToList();
        foreach (var addingTrackId in addingTrackIds)
        {
            var isTrackExistResult = await _tracksQueriesRepository.IsTrackExistsAsync(addingTrackId);
            if (isTrackExistResult.IsFailed)
            {
                return isTrackExistResult.ToResult();
            }

            if (!isTrackExistResult.ValueOrDefault)
            {
                throw new NotImplementedException();
            }
        }

        var addTracksResult = album.AddTracks(addingTrackIds);
        if (addTracksResult.IsFailed)
        {
            return addTracksResult;
        }

        return Result.Ok();
    }

    /// <summary>
    /// Удаление музыкальных треков из музыкального альбома.
    /// </summary>
    /// <param name="album">Музыкальный альбом.</param>
    /// <param name="ids">Идентификаторы, удаляемых музыкальных треков.</param>
    private static Result RemoveTracks(Album album, IEnumerable<Guid> ids)
    {
        var removingTrackIds = ids.Select(TrackId.Create).ToList();

        var removeTracksResult = album.RemoveTracks(removingTrackIds);
        if (removeTracksResult.IsFailed)
        {
            return removeTracksResult;
        }

        return Result.Ok();
    }
}