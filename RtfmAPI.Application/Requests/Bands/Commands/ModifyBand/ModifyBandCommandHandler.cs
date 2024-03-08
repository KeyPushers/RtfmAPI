using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RtfmAPI.Application.Interfaces.Persistence.Commands;
using RtfmAPI.Application.Interfaces.Persistence.Queries;
using RtfmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Domain.Models.Bands;
using RtfmAPI.Domain.Models.Bands.ValueObjects;
using RtfmAPI.Domain.Models.Genres.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Application.Requests.Bands.Commands.ModifyBand;

/// <summary>
/// Обработчик команды изменения музыкальной группы.
/// </summary>
public class ModifyBandCommandHandler : IRequestHandler<ModifyBandCommand, BaseResult>
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
    public async Task<BaseResult> Handle(ModifyBandCommand request, CancellationToken cancellationToken = default)
    {
        var bandId = BandId.Create(request.BandId);
        var getBandResult = await _bandsQueriesRepository.GetBandByIdAsync(bandId);
        if (getBandResult.IsFailed)
        {
            return getBandResult.Error;
        }

        var band = getBandResult.Value;

        if (request.Name is not null)
        {
            var setBandNameResult = SetBandName(band, request.Name);
            if (setBandNameResult.IsFailed)
            {
                return setBandNameResult.Error;
            }
        }

        if (request.AddingAlbumsIds.Any())
        {
            var addAlbumsResult = await AddAlbumsAsync(band, request.AddingAlbumsIds);
            if (addAlbumsResult.IsFailed)
            {
                return addAlbumsResult.Error;
            }
        }

        if (request.RemovingAlbumsIds.Any())
        {
            var removeAlbumsResult =
                await RemoveAlbumsAsync(band, request.RemovingAlbumsIds);
            if (removeAlbumsResult.IsFailed)
            {
                return removeAlbumsResult.Error;
            }
        }

        if (request.AddingGenresIds.Any())
        {
            var addGenresResult = await AddGenresAsync(band, request.AddingGenresIds);
            if (addGenresResult.IsFailed)
            {
                return addGenresResult.Error;
            }
        }

        if (request.RemovingGenresIds.Any())
        {
            var removeGenresResult =
                await RemoveGenresAsync(band, request.RemovingGenresIds);
            if (removeGenresResult.IsFailed)
            {
                return removeGenresResult.Error;
            }
        }
        
        await _bandsCommandsRepository.CommitChangesAsync(band, cancellationToken);
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
        List<AlbumId> addingAlbumIds = new();
        foreach (var addingAlbumId in addingAlbumsIds)
        {
            var albumId = AlbumId.Create(addingAlbumId);
            var isAlbumExists = await _albumsQueriesRepository.IsAlbumExistsAsync(albumId);
            if (!isAlbumExists)
            {
                return new InvalidOperationException();
            }

            addingAlbumIds.Add(albumId);
        }

        var addAlbumsResult = band.AddAlbums(addingAlbumIds);
        return addAlbumsResult.IsFailed ? addAlbumsResult.Error : BaseResult.Success();
    }

    /// <summary>
    /// Удаление музыкальных альбмоов из музыкальной группы.
    /// </summary>
    /// <param name="band">Музыкальная группа.</param>
    /// <param name="removingAlbumsIds">Идентификаторы удаляемых музыкальных альбомов.</param>
    private Task<BaseResult> RemoveAlbumsAsync(Band band, IEnumerable<Guid> removingAlbumsIds)
    {
        var removingAlbums = removingAlbumsIds.Select(AlbumId.Create)
            .Select(albumId => albumId).ToList();

        var removeAlbumsResult = band.RemoveAlbums(removingAlbums);
        return Task.FromResult(removeAlbumsResult.IsFailed ? removeAlbumsResult.Error : BaseResult.Success());
    }

    /// <summary>
    /// Добавление музыкальных жанров в музыкальную группу.
    /// </summary>
    /// <param name="band">Музыкальная группа.</param>
    /// <param name="addingGenresIds">Идентификаторы добавляемых музыкальных жанров.</param>
    private async Task<BaseResult> AddGenresAsync(Band band, IEnumerable<Guid> addingGenresIds)
    {
        List<GenreId> addingGenreIds = new();
        foreach (var addingGenreId in addingGenresIds)
        {
            var genreId = GenreId.Create(addingGenreId);
            var isGenreExists = await _genresQueriesRepository.IsGenreExistsAsync(genreId);
            if (!isGenreExists)
            {
                return new InvalidOperationException();
            }

            addingGenreIds.Add(genreId);
        }

        var addGenresResult = band.AddGenres(addingGenreIds);
        return addGenresResult.IsFailed ? addGenresResult.Error : BaseResult.Success();
    }

    /// <summary>
    /// Удаление музыкальных жанров из музыкальной группы.
    /// </summary>
    /// <param name="band">Музыкальная группа.</param>
    /// <param name="removingGenresIds">Идентификаторы удаляемых музыкальных жанров.</param>
    private Task<BaseResult> RemoveGenresAsync(Band band, IEnumerable<Guid> removingGenresIds)
    {
        var removingGenres = removingGenresIds.Select(GenreId.Create)
            .Select(albumId => albumId).ToList();

        var removeGenresResult = band.RemoveGenres(removingGenres);
        return Task.FromResult(removeGenresResult.IsFailed ? removeGenresResult.Error : BaseResult.Success());
    }
}