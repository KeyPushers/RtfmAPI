using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using RftmAPI.Domain.Exceptions.BandExceptions;
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
            return BandExceptions.NotFound(bandId);
        }

        if (request.Name is not null)
        {
            var setBandNameResult = SetBandName(band, request.Name);
            if (setBandNameResult.IsFailed)
            {
                return setBandNameResult.Error;
            }
        }

        if (request.AddingAlbumsIds is not null)
        {
            var addAlbumsResult = await AddAlbumsAsync(band, request.AddingAlbumsIds, _albumsRepository, _logger);
            if (addAlbumsResult.IsFailed)
            {
                return addAlbumsResult.Error;
            }
        }

        if (request.RemovingAlbumsIds is not null)
        {
            var removeAlbumsResult =
                await RemoveAlbumsAsync(band, request.RemovingAlbumsIds, _albumsRepository, _logger);
            if (removeAlbumsResult.IsFailed)
            {
                return removeAlbumsResult.Error;
            }
        }

        if (request.AddingGenresIds is not null)
        {
            var addGenresResult = await AddGenresAsync(band, request.AddingGenresIds, _genresRepository, _logger);
            if (addGenresResult.IsFailed)
            {
                return addGenresResult.Error;
            }
        }

        if (request.RemovingGenresIds is not null)
        {
            var removeGenresResult =
                await RemoveGenresAsync(band, request.RemovingGenresIds, _genresRepository, _logger);
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
        var bandNameResult = BandName.Create(name);
        if (bandNameResult.IsFailed)
        {
            return bandNameResult.Error;
        }

        var setBandNameResult = band.SetName(bandNameResult.Value);
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
    /// <param name="albumsRepository">Репозиторий музыкальных альбомов.</param>
    /// <param name="logger">Логгер</param>
    private static async Task<BaseResult> AddAlbumsAsync(Band band, IEnumerable<Guid> addingAlbumsIds,
        IAlbumsRepository albumsRepository, ILogger logger)
    {
        List<Album> addingAlbums = new();
        foreach (var addingAlbumId in addingAlbumsIds)
        {
            var albumId = AlbumId.Create(addingAlbumId);
            var album = await albumsRepository.GetAlbumByIdAsync(albumId);
            if (album is null)
            {
                logger.LogWarning(
                    "Не удалось добавить музыкальный альбом {AddingAlbumId} в музыкальную группу {BandId}",
                    addingAlbumId, band.Id.Value);
                continue;
            }

            addingAlbums.Add(album);
        }

        var addAlbumsResult = band.AddAlbums(addingAlbums);
        if (addAlbumsResult.IsFailed)
        {
            return addAlbumsResult.Error;
        }

        return BaseResult.Success();
    }

    /// <summary>
    /// Удаление музыкальных альбмоов из музыкальной группы.
    /// </summary>
    /// <param name="band">Музыкальная группа.</param>
    /// <param name="removingAlbumsIds">Идентификаторы удаляемых музыкальных альбомов.</param>
    /// <param name="albumsRepository">Репозиторий музыкальных альбомов.</param>
    /// <param name="logger">Логгер.</param>
    private static async Task<BaseResult> RemoveAlbumsAsync(Band band, IEnumerable<Guid> removingAlbumsIds,
        IAlbumsRepository albumsRepository, ILogger logger)
    {
        List<Album> removingAlbums = new();
        foreach (var removingAlbumId in removingAlbumsIds)
        {
            var albumId = AlbumId.Create(removingAlbumId);
            var album = await albumsRepository.GetAlbumByIdAsync(albumId);
            if (album is null)
            {
                logger.LogWarning(
                    "Не удалось удалить музыкальный альбом {RemovingAlbumId} из музыкальной группы {BandId}",
                    removingAlbumId, band.Id.Value);
                continue;
            }

            removingAlbums.Add(album);
        }

        var removeAlbumsResult = band.RemoveAlbums(removingAlbums);
        if (removeAlbumsResult.IsFailed)
        {
            return removeAlbumsResult.Error;
        }

        return BaseResult.Success();
    }

    /// <summary>
    /// Добавление музыкальных жанров в музыкальную группу.
    /// </summary>
    /// <param name="band">Музыкальная группа.</param>
    /// <param name="addingGenresIds">Идентификаторы добавляемых музыкальных жанров.</param>
    /// <param name="genresRepository">Репозиторий музыкальных жанров.</param>
    /// <param name="logger">Логгер.</param>
    private static async Task<BaseResult> AddGenresAsync(Band band, IEnumerable<Guid> addingGenresIds,
        IGenresRepository genresRepository, ILogger logger)
    {
        List<Genre> addingGenres = new();
        foreach (var addingGenreId in addingGenresIds)
        {
            var genreId = GenreId.Create(addingGenreId);
            var genre = await genresRepository.GetGenreByIdAsync(genreId);
            if (genre is null)
            {
                logger.LogWarning("Не удалось добавить музыкальный жанр {AddingGenreId} в музыкальную группу {BandId}",
                    addingGenreId, band.Id.Value);
                continue;
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
    /// <param name="genresRepository">Репозиторий музыкальных жанров.</param>
    /// <param name="logger">Логгер.</param>
    private static async Task<BaseResult> RemoveGenresAsync(Band band, IEnumerable<Guid> removingGenresIds,
        IGenresRepository genresRepository, ILogger logger)
    {
        List<Genre> removingGenres = new();
        foreach (var removingGenreId in removingGenresIds)
        {
            var genreId = GenreId.Create(removingGenreId);
            var genre = await genresRepository.GetGenreByIdAsync(genreId);
            if (genre is null)
            {
                logger.LogWarning(
                    "Не удалось удалить музыкальный жанр {RemovingGenreId} из музыкальной группы {BandId}",
                    removingGenreId, band.Id.Value);
                continue;
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