using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using RftmAPI.Domain.Exceptions.AlbumExceptions;
using RftmAPI.Domain.Exceptions.BandExceptions;
using RftmAPI.Domain.Exceptions.TrackExceptions;
using RftmAPI.Domain.Models.Albums;
using RftmAPI.Domain.Models.Albums.ValueObjects;
using RftmAPI.Domain.Models.Bands;
using RftmAPI.Domain.Models.Bands.ValueObjects;
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
    private readonly IBandsRepository _bandsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ModifyAlbumCommandHandler> _logger;

    /// <summary>
    /// Создание обработчика команды изменения музыкального альбома.
    /// </summary>
    /// <param name="albumsRepository">Репозиторий музыкального альбома.</param>
    /// <param name="tracksRepository">Репозиторий музыкального трека.</param>
    /// <param name="bandsRepository">Репозиторий музыкальной группы.</param>
    /// <param name="unitOfWork">Единица работы.</param>
    /// <param name="logger">Логгер.</param>
    public ModifyAlbumCommandHandler(IAlbumsRepository albumsRepository, ITracksRepository tracksRepository,
        IBandsRepository bandsRepository, IUnitOfWork unitOfWork, ILogger<ModifyAlbumCommandHandler> logger)
    {
        _albumsRepository = albumsRepository;
        _tracksRepository = tracksRepository;
        _bandsRepository = bandsRepository;
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

        if (request.AddingBandsIds is not null && request.AddingBandsIds.Any())
        {
            var addBandsResult = await AddBandsAsync(album, request.AddingBandsIds);
            if (addBandsResult.IsFailed)
            {
                return addBandsResult.Error;
            }
        }

        if (request.RemovingBandsIds is not null && request.RemovingBandsIds.Any())
        {
            var removeBandsResult = await RemoveBandsAsync(album, request.RemovingBandsIds);
            if (removeBandsResult.IsFailed)
            {
                return removeBandsResult.Error;
            }
        }

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
        if (setAlbumNameResult.IsFailed)
        {
            return setAlbumNameResult.Error;
        }

        return BaseResult.Success();
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
        if (albumReleaseDateCreateResult.IsFailed)
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
                // TODO: Добавление в ресурсы.
                _logger.LogError(error,
                    "Не удалось добавить музыкальный трек {AddingTrackId} в музыкальный альбом {AlbumId}",
                    trackId.Value, album.Id.Value);
                return error;
            }

            addingTracks.Add(track);
        }

        var addTracksResult = album.AddTracks(addingTracks);
        if (addTracksResult.IsFailed)
        {
            return addTracksResult.Error;
        }

        foreach (var addingTrack in addingTracks)
        {
            var addAlbumResult = addingTrack.AddAlbum(album);
            if (addAlbumResult.IsFailed)
            {
                return addAlbumResult.Error;
            }
        }

        return BaseResult.Success();
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
                // TODO: Добавление в ресурсы.
                _logger.LogError(error,
                    "Не удалось удалить музыкальный трек {RemovingTrackId} из музыкального альбома {AlbumId}",
                    trackId.Value, album.Id.Value);
                return error;
            }

            removingTracks.Add(track);
        }

        var removeTracksResult = album.RemoveTracks(removingTracks);
        if (removeTracksResult.IsFailed)
        {
            return removeTracksResult.Error;
        }

        foreach (var removingTrack in removingTracks)
        {
            var removingTrackRemoveAlbumResult = removingTrack.RemoveAlbum();
            if (removingTrackRemoveAlbumResult.IsFailed)
            {
                return removingTrackRemoveAlbumResult.Error;
            }
        }

        return BaseResult.Success();
    }

    /// <summary>
    /// Добавление музыкальных группы в музыкальный альбом.
    /// </summary>
    /// <param name="album">Музыкальный альбом.</param>
    /// <param name="addingBandsIds">Идентификаторы, добавляемых музыкальных групп.</param>
    private async Task<BaseResult> AddBandsAsync(Album album, IEnumerable<Guid> addingBandsIds)
    {
        List<Band> addingBands = new();
        foreach (var addingBandId in addingBandsIds)
        {
            var bandId = BandId.Create(addingBandId);
            var band = await _bandsRepository.GetBandByIdAsync(bandId);
            if (band is null)
            {
                var error = BandExceptions.NotFound(bandId);
                // TODO: Добавление в ресурсы.
                _logger.LogError(error,
                    "Не удалось добавить музыкальную группу {AddingBandId} в музыкальный альбом {AlbumId}",
                    bandId.Value, album.Id.Value);
                return error;
            }

            addingBands.Add(band);
        }

        var addBandsResult = album.AddBands(addingBands);
        if (addBandsResult.IsFailed)
        {
            return addBandsResult.Error;
        }

        var addingAlbumToBands = new[] {album};
        foreach (var addingBand in addingBands)
        {
            var addAlbumResult = addingBand.AddAlbums(addingAlbumToBands);
            if (addAlbumResult.IsFailed)
            {
                return addAlbumResult.Error;
            }
        }

        return BaseResult.Success();
    }

    /// <summary>
    /// Удаление музыкальных групп из музыкального альбома.
    /// </summary>
    /// <param name="album">Музыкальный альбом.</param>
    /// <param name="removingBandsIds">Идентификаторы, удаляемых музыкальных групп.</param>
    private async Task<BaseResult> RemoveBandsAsync(Album album, IEnumerable<Guid> removingBandsIds)
    {
        List<Band> removingBands = new();
        foreach (var removingBandId in removingBandsIds)
        {
            var bandId = BandId.Create(removingBandId);
            var band = await _bandsRepository.GetBandByIdAsync(bandId);
            if (band is null)
            {
                var error = BandExceptions.NotFound(bandId);
                // TODO: Добавление в ресурсы.
                _logger.LogError(error,
                    "Не удалось удалить музыкальную группу {RemovingBandId} из музыкального альбома {AlbumId}",
                    bandId.Value, album.Id.Value);
                return error;
            }
            
            removingBands.Add(band);
        }

        var removeBandsResult = album.RemoveBands(removingBands);
        if (removeBandsResult.IsFailed)
        {
            return removeBandsResult.Error;
        }

        var removingAlbumFromBands = new[] {album};
        foreach (var removingBand in removingBands)
        {
            var removeAlbumResult = removingBand.RemoveAlbums(removingAlbumFromBands);
            if (removeAlbumResult.IsFailed)
            {
                return removeAlbumResult.Error;
            }
        }
        
        return BaseResult.Success();
    }
}