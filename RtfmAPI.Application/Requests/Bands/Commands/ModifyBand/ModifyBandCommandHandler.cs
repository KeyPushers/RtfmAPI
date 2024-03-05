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
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Application.Requests.Bands.Commands.ModifyBand;

/// <summary>
/// Обработчик команды изменения музыкальной группы.
/// </summary>
public class ModifyBandCommandHandler : IRequestHandler<ModifyBandCommand, BaseResult>
{
    private readonly IBandsQueriesRepository _bandsQueriesRepository;
    private readonly IBandsCommandsRepository _bandsCommandsRepository;
    private readonly IAlbumsQueriesRepository _albumsQueriesRepository;

    /// <summary>
    /// Создание обработчика команды изменения музыкальной группы.
    /// </summary>
    /// <param name="bandsQueriesRepository">Репозиторий запросов музыкальных групп.</param>
    /// <param name="bandsCommandsRepository">Репозиторий команд музыкальных групп.</param>
    /// <param name="albumsQueriesRepository">Репозиторий запросов музыкальных альбомов.</param>
    public ModifyBandCommandHandler(IBandsQueriesRepository bandsQueriesRepository,
        IBandsCommandsRepository bandsCommandsRepository, IAlbumsQueriesRepository albumsQueriesRepository)
    {
        _bandsQueriesRepository = bandsQueriesRepository;
        _bandsCommandsRepository = bandsCommandsRepository;
        _albumsQueriesRepository = albumsQueriesRepository;
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


        await _bandsCommandsRepository.CommitChangesAsync(band);
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
                return new NotImplementedException();
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
        var removingAlbumIds = removingAlbumsIds.Select(AlbumId.Create)
            .Select(albumId => albumId).ToList();

        var removeAlbumsResult = band.RemoveAlbums(removingAlbumIds);
        return Task.FromResult(removeAlbumsResult.IsFailed ? removeAlbumsResult.Error : BaseResult.Success());
    }
}