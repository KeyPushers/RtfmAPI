using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using RftmAPI.Domain.Exceptions.AlbumExceptions;
using RftmAPI.Domain.Exceptions.BandExceptions;
using RftmAPI.Domain.Exceptions.GenreExceptions;
using RftmAPI.Domain.Models.Albums;
using RftmAPI.Domain.Models.Albums.ValueObjects;
using RftmAPI.Domain.Models.Bands;
using RftmAPI.Domain.Models.Bands.ValueObjects;
using RftmAPI.Domain.Models.Genres;
using RftmAPI.Domain.Models.Genres.ValueObjects;
using RftmAPI.Domain.Primitives;
using RtfmAPI.Application.Common.Interfaces.Persistence;

namespace RtfmAPI.Application.Requests.Bands.Commands.ModifyBand;

/// <summary>
/// Обработчик команды изменения музыкальной группы.
/// </summary>
public class ModifyBandCommandHandler : IRequestHandler<ModifyBandCommand, BaseResult>
{
    private readonly IBandsRepository _bandsRepository;
    private readonly IAlbumsRepository _albumsRepository;
    private readonly IGenresRepository _genresRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ModifyBandCommandHandler> _logger;

    /// <summary>
    /// Создание обработчика команды изменения музыкальной группы.
    /// </summary>
    /// <param name="bandsRepository">Репозиторий музыкальной группы.</param>
    /// <param name="albumsRepository">Репозиторий музыкальных альбомов.</param>
    /// <param name="genresRepository">Репозиторий музыкальных жанров.</param>
    /// <param name="unitOfWork">Единица работы.</param>
    /// <param name="logger">Логгер.</param>
    public ModifyBandCommandHandler(IBandsRepository bandsRepository, IAlbumsRepository albumsRepository,
        IGenresRepository genresRepository, IUnitOfWork unitOfWork, ILogger<ModifyBandCommandHandler> logger)
    {
        _bandsRepository = bandsRepository;
        _albumsRepository = albumsRepository;
        _genresRepository = genresRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Обработка команды изменения музыкальной группы.
    /// </summary>
    /// <param name="request">Команда изменения музыкальной группы.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Объект переноса данных команды изменения музыкальной группы.</returns>
    public async Task<BaseResult> Handle(ModifyBandCommand request, CancellationToken cancellationToken = default)
    {
        var bandId = BandId.Create(request.BandId);
        var band = await _bandsRepository.GetBandByIdAsync(bandId);
        if (band is null)
        {
            var error = BandExceptions.NotFound(bandId);
            // TODO: Добавить в ресурсы.
            _logger.LogError(error, "Не удалось изменить музыкальную группу {BandId}", bandId.Value);
            return error;
        }

        if (request.Name is not null)
        {
            var setBandNameResult = SetBandName(band, request.Name);
            if (setBandNameResult.IsFailed)
            {
                return setBandNameResult.Error;
            }
        }

        if (request.AddingAlbumsIds is not null && request.AddingAlbumsIds.Any())
        {
            var addAlbumsResult = await AddAlbumsAsync(band, request.AddingAlbumsIds);
            if (addAlbumsResult.IsFailed)
            {
                return addAlbumsResult.Error;
            }
        }

        if (request.RemovingAlbumsIds is not null && request.RemovingAlbumsIds.Any())
        {
            var removeAlbumsResult =
                await RemoveAlbumsAsync(band, request.RemovingAlbumsIds);
            if (removeAlbumsResult.IsFailed)
            {
                return removeAlbumsResult.Error;
            }
        }

        if (request.AddingGenresIds is not null && request.AddingGenresIds.Any())
        {
            var addGenresResult = await AddGenresAsync(band, request.AddingGenresIds);
            if (addGenresResult.IsFailed)
            {
                return addGenresResult.Error;
            }
        }

        if (request.RemovingGenresIds is not null && request.RemovingGenresIds.Any())
        {
            var removeGenresResult =
                await RemoveGenresAsync(band, request.RemovingGenresIds);
            if (removeGenresResult.IsFailed)
            {
                return removeGenresResult.Error;
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return BaseResult.Success();
    }

    /// <summary>
    /// Установка названия музыкальной группы.
    /// </summary>
    /// <param name="band">Музыкальная группа.</param>
    /// <param name="name">Название музыкальной группы.</param>
    private static BaseResult SetBandName(Band band, string name)
    {
        var bandNameCreateResult = BandName.Create(name);
        if (bandNameCreateResult.IsFailed)
        {
            return bandNameCreateResult.Error;
        }

        var setBandNameResult = band.SetName(bandNameCreateResult.Value);
        if (setBandNameResult.IsFailed)
        {
            return setBandNameResult.Error;
        }

        return BaseResult.Success();
    }

    /// <summary>
    /// Добавление музыкальных альбомов в музыкальную группу.
    /// </summary>
    /// <param name="band">Музыкальная группа.</param>
    /// <param name="addingAlbumsIds">Идентификаторы добавляемых музыкальных альбомов.</param>
    private async Task<BaseResult> AddAlbumsAsync(Band band, IEnumerable<Guid> addingAlbumsIds)
    {
        List<Album> addingAlbums = new();
        foreach (var addingAlbumId in addingAlbumsIds)
        {
            var albumId = AlbumId.Create(addingAlbumId);
            var album = await _albumsRepository.GetAlbumByIdAsync(albumId);
            if (album is null)
            {
                var error = AlbumExceptions.NotFound(albumId);
                // TODO: Добавление в ресурсы.
                _logger.LogError(error,
                    "Не удалось добавить музыкальный альбом {AddingAlbumId} в музыкальную группу {BandId}",
                    albumId.Value, band.Id.Value);
                return error;
            }

            addingAlbums.Add(album);
        }

        var addAlbumsResult = band.AddAlbums(addingAlbums);
        if (addAlbumsResult.IsFailed)
        {
            return addAlbumsResult.Error;
        }

        var addingBandToAlbums = new[] {band};
        foreach (var addingAlbum in addingAlbums)
        {
            var albumAddBandsResult = addingAlbum.AddBands(addingBandToAlbums);
            if (albumAddBandsResult.IsFailed)
            {
                return albumAddBandsResult.Error;
            }
        }

        return BaseResult.Success();
    }

    /// <summary>
    /// Удаление музыкальных альбмоов из музыкальной группы.
    /// </summary>
    /// <param name="band">Музыкальная группа.</param>
    /// <param name="removingAlbumsIds">Идентификаторы удаляемых музыкальных альбомов.</param>
    private async Task<BaseResult> RemoveAlbumsAsync(Band band, IEnumerable<Guid> removingAlbumsIds)
    {
        List<Album> removingAlbums = new();
        foreach (var removingAlbumId in removingAlbumsIds)
        {
            var albumId = AlbumId.Create(removingAlbumId);
            var album = await _albumsRepository.GetAlbumByIdAsync(albumId);
            if (album is null)
            {
                var error = AlbumExceptions.NotFound(albumId);
                // TODO: Добавление в ресурсы.
                _logger.LogError(error,
                    "Не удалось удалить музыкальный альбом {RemovingAlbumId} из музыкальной группы {BandId}",
                    albumId.Value, band.Id.Value);
                return error;
            }

            removingAlbums.Add(album);
        }

        var removeAlbumsResult = band.RemoveAlbums(removingAlbums);
        if (removeAlbumsResult.IsFailed)
        {
            return removeAlbumsResult.Error;
        }

        var removingBandFromAlbums = new[] {band};
        foreach (var removingAlbum in removingAlbums)
        {
            var albumRemoveBandsResult = removingAlbum.RemoveBands(removingBandFromAlbums);
            if (albumRemoveBandsResult.IsFailed)
            {
                return albumRemoveBandsResult.Error;
            }
        }

        return BaseResult.Success();
    }

    /// <summary>
    /// Добавление музыкальных жанров в музыкальную группу.
    /// </summary>
    /// <param name="band">Музыкальная группа.</param>
    /// <param name="addingGenresIds">Идентификаторы добавляемых музыкальных жанров.</param>
    private async Task<BaseResult> AddGenresAsync(Band band, IEnumerable<Guid> addingGenresIds)
    {
        List<Genre> addingGenres = new();
        foreach (var addingGenreId in addingGenresIds)
        {
            var genreId = GenreId.Create(addingGenreId);
            var genre = await _genresRepository.GetGenreByIdAsync(genreId);
            if (genre is null)
            {
                var error = GenreExceptions.NotFound(genreId);
                // TODO: Добавление в ресурсы.
                _logger.LogError(error,
                    "Не удалось добавить музыкальный жанр {AddingGenreId} в музыкальную группу {BandId}",
                    addingGenreId, band.Id.Value);
                return error;
            }

            addingGenres.Add(genre);
        }

        var addGenresResult = band.AddGenres(addingGenres);
        if (addGenresResult.IsFailed)
        {
            return addGenresResult.Error;
        }

        return BaseResult.Success();
    }

    /// <summary>
    /// Удаление музыкальных жанров из музыкальной группы.
    /// </summary>
    /// <param name="band">Музыкальная группа.</param>
    /// <param name="removingGenresIds">Идентификаторы удаляемых музыкальных жанров.</param>
    private async Task<BaseResult> RemoveGenresAsync(Band band, IEnumerable<Guid> removingGenresIds)
    {
        List<Genre> removingGenres = new();
        foreach (var removingGenreId in removingGenresIds)
        {
            var genreId = GenreId.Create(removingGenreId);
            var genre = await _genresRepository.GetGenreByIdAsync(genreId);
            if (genre is null)
            {
                var error = GenreExceptions.NotFound(genreId);
                // TODO: Добавление в ресурсы.
                _logger.LogError(error,
                    "Не удалось удалить музыкальный жанр {RemovingGenreId} из музыкальной группы {BandId}",
                    removingGenreId, band.Id.Value);
                return error;
            }

            removingGenres.Add(genre);
        }

        var removeGenresResult = band.RemoveGenres(removingGenres);
        if (removeGenresResult.IsFailed)
        {
            return removeGenresResult.Error;
        }

        return BaseResult.Success();
    }
}