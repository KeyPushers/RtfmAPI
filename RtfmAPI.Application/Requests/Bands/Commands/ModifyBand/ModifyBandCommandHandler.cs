using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using MediatR;
using RtfmAPI.Application.Interfaces.Persistence.Commands;
using RtfmAPI.Application.Interfaces.Persistence.Queries;
using RtfmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Domain.Models.Bands;
using RtfmAPI.Domain.Models.Bands.ValueObjects;
using RtfmAPI.Domain.Models.Genres.ValueObjects;

namespace RtfmAPI.Application.Requests.Bands.Commands.ModifyBand;

/// <summary>
/// Обработчик команды изменения музыкальной группы.
/// </summary>
public class ModifyBandCommandHandler : IRequestHandler<ModifyBandCommand, Result>
{
    private readonly IBandsCommandsRepository _bandsCommandsRepository;
    private readonly IBandsQueriesRepository _bandsQueriesRepository;
    private readonly IAlbumsQueriesRepository _albumsQueriesRepository;
    private readonly IGenresQueriesRepository _genresQueriesRepository;

    /// <summary>
    /// Создание обработчика команды изменения музыкальной группы.
    /// </summary>
    /// <param name="bandsCommandsRepository">Репозиторий команд музыкальных групп.</param>
    /// <param name="bandsQueriesRepository">Репозиторий запросов музыкальных групп.</param>
    /// <param name="albumsQueriesRepository">Репозиторий запросов музыкальных альбомов.</param>
    /// <param name="genresQueriesRepository">репозиторий запросов музыкальных жанров.</param>
    public ModifyBandCommandHandler(IBandsCommandsRepository bandsCommandsRepository,
        IBandsQueriesRepository bandsQueriesRepository, IAlbumsQueriesRepository albumsQueriesRepository,
        IGenresQueriesRepository genresQueriesRepository)
    {
        _bandsQueriesRepository = bandsQueriesRepository;
        _bandsCommandsRepository = bandsCommandsRepository;
        _albumsQueriesRepository = albumsQueriesRepository;
        _genresQueriesRepository = genresQueriesRepository;
    }

    /// <summary>
    /// Обработка команды изменения музыкальной группы.
    /// </summary>
    /// <param name="request">Команда изменения музыкальной группы.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Объект переноса данных команды изменения музыкальной группы.</returns>
    public async Task<Result> Handle(ModifyBandCommand request, CancellationToken cancellationToken = default)
    {
        var bandId = BandId.Create(request.BandId);
        var getBandResult = await _bandsQueriesRepository.GetBandByIdAsync(bandId);
        if (getBandResult.IsFailed)
        {
            return getBandResult.ToResult();
        }

        var band = getBandResult.ValueOrDefault;

        if (request.Name is not null)
        {
            var setBandNameResult = SetBandName(band, request.Name);
            if (setBandNameResult.IsFailed)
            {
                return setBandNameResult;
            }
        }

        if (request.AddingAlbumsIds.Any())
        {
            var addAlbumsResult = await AddAlbumsAsync(band, request.AddingAlbumsIds);
            if (addAlbumsResult.IsFailed)
            {
                return addAlbumsResult;
            }
        }

        if (request.RemovingAlbumsIds.Any())
        {
            var removeAlbumsResult = RemoveAlbums(band, request.RemovingAlbumsIds);
            if (removeAlbumsResult.IsFailed)
            {
                return removeAlbumsResult;
            }
        }

        if (request.AddingGenresIds.Any())
        {
            var addGenresResult = await AddGenresAsync(band, request.AddingGenresIds);
            if (addGenresResult.IsFailed)
            {
                return addGenresResult;
            }
        }

        if (request.RemovingGenresIds.Any())
        {
            var removeGenresResult = RemoveGenres(band, request.RemovingGenresIds);
            if (removeGenresResult.IsFailed)
            {
                return removeGenresResult;
            }
        }

        await _bandsCommandsRepository.CommitChangesAsync(band, cancellationToken);
        return Result.Ok();
    }

    /// <summary>
    /// Установка названия музыкальной группы.
    /// </summary>
    /// <param name="band">Музыкальная группа.</param>
    /// <param name="name">Название музыкальной группы.</param>
    private static Result SetBandName(Band band, string name)
    {
        var bandNameCreateResult = BandName.Create(name);
        if (bandNameCreateResult.IsFailed)
        {
            return bandNameCreateResult.ToResult();
        }

        var setBandNameResult = band.SetName(bandNameCreateResult.Value);
        if (setBandNameResult.IsFailed)
        {
            return setBandNameResult;
        }

        return Result.Ok();
    }

    /// <summary>
    /// Добавление музыкальных альбомов в музыкальную группу.
    /// </summary>
    /// <param name="band">Музыкальная группа.</param>
    /// <param name="addingAlbumsIds">Идентификаторы добавляемых музыкальных альбомов.</param>
    private async Task<Result> AddAlbumsAsync(Band band, IEnumerable<Guid> addingAlbumsIds)
    {
        List<AlbumId> addingAlbumIds = new();
        foreach (var addingAlbumId in addingAlbumsIds)
        {
            var albumId = AlbumId.Create(addingAlbumId);
            var isAlbumExistResult = await _albumsQueriesRepository.IsAlbumExistsAsync(albumId);
            if (isAlbumExistResult.IsFailed)
            {
                return isAlbumExistResult.ToResult();
            }

            if (!isAlbumExistResult.ValueOrDefault)
            {
                throw new NotImplementedException();
            }

            addingAlbumIds.Add(albumId);
        }

        var addAlbumsResult = band.AddAlbums(addingAlbumIds);
        if (addAlbumsResult.IsFailed)
        {
            return addAlbumsResult;
        }

        return Result.Ok();
    }

    /// <summary>
    /// Удаление музыкальных альбмоов из музыкальной группы.
    /// </summary>
    /// <param name="band">Музыкальная группа.</param>
    /// <param name="removingAlbumsIds">Идентификаторы удаляемых музыкальных альбомов.</param>
    private static Result RemoveAlbums(Band band, IEnumerable<Guid> removingAlbumsIds)
    {
        var removingAlbums = removingAlbumsIds.Select(AlbumId.Create)
            .Select(albumId => albumId).ToList();

        var removeAlbumsResult = band.RemoveAlbums(removingAlbums);
        if (removeAlbumsResult.IsFailed)
        {
            return removeAlbumsResult;
        }

        return Result.Ok();
    }

    /// <summary>
    /// Добавление музыкальных жанров в музыкальную группу.
    /// </summary>
    /// <param name="band">Музыкальная группа.</param>
    /// <param name="addingGenresIds">Идентификаторы добавляемых музыкальных жанров.</param>
    private async Task<Result> AddGenresAsync(Band band, IEnumerable<Guid> addingGenresIds)
    {
        List<GenreId> addingGenreIds = new();
        foreach (var addingGenreId in addingGenresIds)
        {
            var genreId = GenreId.Create(addingGenreId);
            var isGenreExistResult = await _genresQueriesRepository.IsGenreExistsAsync(genreId);
            if (isGenreExistResult.IsFailed)
            {
                return isGenreExistResult.ToResult();
            }

            if (!isGenreExistResult.ValueOrDefault)
            {
                throw new NotImplementedException();
            }

            addingGenreIds.Add(genreId);
        }

        var addGenresResult = band.AddGenres(addingGenreIds);
        if (addGenresResult.IsFailed)
        {
            return addGenresResult;
        }

        return Result.Ok();
    }

    /// <summary>
    /// Удаление музыкальных жанров из музыкальной группы.
    /// </summary>
    /// <param name="band">Музыкальная группа.</param>
    /// <param name="removingGenresIds">Идентификаторы удаляемых музыкальных жанров.</param>
    private static Result RemoveGenres(Band band, IEnumerable<Guid> removingGenresIds)
    {
        var removingGenres = removingGenresIds.Select(GenreId.Create)
            .Select(albumId => albumId).ToList();

        var removeGenresResult = band.RemoveGenres(removingGenres);
        if (removeGenresResult.IsFailed)
        {
            return removeGenresResult;
        }

        return Result.Ok();
    }
}