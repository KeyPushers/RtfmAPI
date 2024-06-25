using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RtfmAPI.Application.Interfaces.Persistence.Commands;
using RtfmAPI.Application.Interfaces.Persistence.Queries;
using RtfmAPI.Domain.Models.Albums;
using RtfmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Domain.Models.Tracks.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Application.Requests.Albums.Commands.ModifyAlbum;

/// <summary>
/// Обработчик команды изменения музыкального альбома.
/// </summary>
public class ModifyAlbumCommandHandler : IRequestHandler<ModifyAlbumCommand, BaseResult>
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
    public async Task<BaseResult> Handle(ModifyAlbumCommand request, CancellationToken cancellationToken = default)
    {
        var albumId = AlbumId.Create(request.AlbumId);
        var getAlbumResult = await _albumsQueriesRepository.GetAlbumByIdAsync(albumId);
        if (getAlbumResult.IsFailed)
        {
            return getAlbumResult.Error;
        }

        var album = getAlbumResult.Value;

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

        await _albumsCommandsRepository.CommitChangesAsync(album, cancellationToken);
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
    /// <param name="ids">Идентификаторы, добавляемых музыкальных треков.</param>
    private async Task<BaseResult> AddTracksAsync(Album album, IEnumerable<Guid> ids)
    {
        var addingTrackIds = ids.Select(TrackId.Create).ToList();
        foreach (var addingTrackId in addingTrackIds)
        {
            if (!await _tracksQueriesRepository.IsTrackExistsAsync(addingTrackId))
            {
                return new InvalidOperationException();
            }
        }

        var addTracksResult = album.AddTracks(addingTrackIds);
        return addTracksResult.IsFailed ? addTracksResult.Error : BaseResult.Success();
    }

    /// <summary>
    /// Удаление музыкальных треков из музыкального альбома.
    /// </summary>
    /// <param name="album">Музыкальный альбом.</param>
    /// <param name="ids">Идентификаторы, удаляемых музыкальных треков.</param>
    private Task<BaseResult> RemoveTracksAsync(Album album, IEnumerable<Guid> ids)
    {
        var removingTrackIds = ids.Select(TrackId.Create).ToList();

        var removeTracksResult = album.RemoveTracks(removingTrackIds);
        return Task.FromResult(removeTracksResult.IsFailed ? removeTracksResult.Error : BaseResult.Success());
    }
}