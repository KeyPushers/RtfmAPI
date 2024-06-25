using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RtfmAPI.Application.Interfaces.Persistence.Commands;
using RtfmAPI.Application.Interfaces.Persistence.Queries;
using RtfmAPI.Domain.Models.Genres.ValueObjects;
using RtfmAPI.Domain.Models.Tracks;
using RtfmAPI.Domain.Models.Tracks.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Application.Requests.Tracks.Commands.ModifyTrack;

/// <summary>
/// Обработчик команды изменения музыкального трека.
/// </summary>
public class ModifyTrackCommandHandler : IRequestHandler<ModifyTrackCommand, BaseResult>
{
    private readonly ITracksCommandsRepository _tracksCommandsRepository;
    private readonly ITracksQueriesRepository _tracksQueriesRepository;
    private readonly IGenresQueriesRepository _genresQueriesRepository;

    /// <summary>
    /// Создание обработчика команды изменения музыкального трека.
    /// </summary>
    /// <param name="tracksCommandsRepository">Репозиторий команд музыкальных треков.</param>
    /// <param name="tracksQueriesRepository">Репозиторий запросов музыкальных треков.</param>
    /// <param name="genresQueriesRepository">Репозиторий запросов музыкальных жанров.</param>
    public ModifyTrackCommandHandler(ITracksCommandsRepository tracksCommandsRepository,
        ITracksQueriesRepository tracksQueriesRepository, IGenresQueriesRepository genresQueriesRepository)
    {
        _tracksCommandsRepository = tracksCommandsRepository;
        _tracksQueriesRepository = tracksQueriesRepository;
        _genresQueriesRepository = genresQueriesRepository;
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
        var getTrackResult = await _tracksQueriesRepository.GetTrackByIdAsync(trackId);
        if (getTrackResult.IsFailed)
        {
            return getTrackResult.Error;
        }

        var track = getTrackResult.Value;
        
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

        await _tracksCommandsRepository.CommitChangesAsync(track, cancellationToken);
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
        return trackSetReleaseDateRelease.IsFailed ? trackSetReleaseDateRelease.Error : BaseResult.Success();
    }

    /// <summary>
    /// Добавление музыкальных жанров к музыкальному треку.
    /// </summary>
    /// <param name="track">Музыкальный трек.</param>
    /// <param name="addingGenresIds">Идентификаторы добавляемых музыкальных жанров</param>
    private async Task<BaseResult> AddGenresAsync(Track track, IEnumerable<Guid> addingGenresIds)
    {
        List<GenreId> addingGenreIds = new();
        foreach (var addingGenreId in addingGenresIds)
        {
            var genreId = GenreId.Create(addingGenreId);
            if (!await _genresQueriesRepository.IsGenreExistsAsync(genreId))
            {
                return new InvalidOperationException();
            }

            addingGenreIds.Add(genreId);
        }

        var addGenresResult = track.AddGenres(addingGenreIds);
        return addGenresResult.IsFailed ? addGenresResult.Error : BaseResult.Success();
    }

    /// <summary>
    /// Удаление музыкальных жанров из музыкального трека.
    /// </summary>
    /// <param name="track">Музыкальный трек.</param>
    /// <param name="removingGenresIds">Идентификаторы удаляемых музыкальных жанров.</param>
    private Task<BaseResult> RemoveGenresAsync(Track track, IEnumerable<Guid> removingGenresIds)
    {
        var removingGenres = removingGenresIds.Select(GenreId.Create)
            .Select(albumId => albumId).ToList();

        var removeGenresResult = track.RemoveGenres(removingGenres);
        return Task.FromResult(removeGenresResult.IsFailed ? removeGenresResult.Error : BaseResult.Success());
    }
}