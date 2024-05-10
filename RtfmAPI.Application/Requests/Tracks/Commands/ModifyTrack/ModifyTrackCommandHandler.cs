using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using MediatR;
using RtfmAPI.Application.Interfaces.Persistence.Commands;
using RtfmAPI.Application.Interfaces.Persistence.Queries;
using RtfmAPI.Domain.Models.Genres.ValueObjects;
using RtfmAPI.Domain.Models.Tracks;
using RtfmAPI.Domain.Models.Tracks.ValueObjects;

namespace RtfmAPI.Application.Requests.Tracks.Commands.ModifyTrack;

/// <summary>
/// Обработчик команды изменения музыкального трека.
/// </summary>
public class ModifyTrackCommandHandler : IRequestHandler<ModifyTrackCommand, Result>
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
    public async Task<Result> Handle(ModifyTrackCommand request, CancellationToken cancellationToken = default)
    {
        var trackId = TrackId.Create(request.TrackId);
        var getTrackResult = await _tracksQueriesRepository.GetTrackByIdAsync(trackId);
        if (getTrackResult.IsFailed)
        {
            return getTrackResult.ToResult();
        }

        var track = getTrackResult.ValueOrDefault;

        if (request.Name is not null)
        {
            var setTrackNameResult = SetTrackName(track, request.Name);
            if (setTrackNameResult.IsFailed)
            {
                return setTrackNameResult;
            }
        }

        if (request.ReleaseDate is not null)
        {
            var setTrackReleaseDateResult = SetTrackReleaseDate(track, request.ReleaseDate.Value);
            if (setTrackReleaseDateResult.IsFailed)
            {
                return setTrackReleaseDateResult;
            }
        }

        if (request.AddingGenresIds is not null && request.AddingGenresIds.Any())
        {
            var addGenresResult = await AddGenresAsync(track, request.AddingGenresIds);
            if (addGenresResult.IsFailed)
            {
                return addGenresResult;
            }
        }

        if (request.RemovingGenresIds is not null && request.RemovingGenresIds.Any())
        {
            var removeGenresResult = RemoveGenres(track, request.RemovingGenresIds);
            if (removeGenresResult.IsFailed)
            {
                return removeGenresResult;
            }
        }

        await _tracksCommandsRepository.CommitChangesAsync(track, cancellationToken);
        return Result.Ok();
    }

    /// <summary>
    /// Изменение названия музыкального трека.
    /// </summary>
    /// <param name="track">Музыкальный трек.</param>
    /// <param name="name">Название музыкального трека.</param>
    private static Result SetTrackName(Track track, string name)
    {
        var trackNameCreateResult = TrackName.Create(name);
        if (trackNameCreateResult.IsFailed)
        {
            return trackNameCreateResult.ToResult();
        }

        var setTrackNameResult = track.SetName(trackNameCreateResult.Value);
        if (setTrackNameResult.IsFailed)
        {
            return setTrackNameResult;
        }

        return Result.Ok();
    }

    /// <summary>
    /// Изменение даты выпуска музыкального трека.
    /// </summary>
    /// <param name="track">Музыкальный трек.</param>
    /// <param name="releaseDate">Дата выпуска музыкального трека.</param>
    private static Result SetTrackReleaseDate(Track track, DateTime releaseDate)
    {
        var trackReleaseDateCreateResult = TrackReleaseDate.Create(releaseDate);
        if (trackReleaseDateCreateResult.IsFailed)
        {
            return trackReleaseDateCreateResult.ToResult();
        }

        var trackSetReleaseDateRelease = track.SetReleaseDate(trackReleaseDateCreateResult.Value);
        if (trackReleaseDateCreateResult.IsFailed)
        {
            return trackSetReleaseDateRelease;
        }

        return Result.Ok();
    }

    /// <summary>
    /// Добавление музыкальных жанров к музыкальному треку.
    /// </summary>
    /// <param name="track">Музыкальный трек.</param>
    /// <param name="addingGenresIds">Идентификаторы добавляемых музыкальных жанров</param>
    private async Task<Result> AddGenresAsync(Track track, IEnumerable<Guid> addingGenresIds)
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

        var addGenresResult = track.AddGenres(addingGenreIds);
        if (addGenresResult.IsFailed)
        {
            return addGenresResult;
        }

        return Result.Ok();
    }

    /// <summary>
    /// Удаление музыкальных жанров из музыкального трека.
    /// </summary>
    /// <param name="track">Музыкальный трек.</param>
    /// <param name="removingGenresIds">Идентификаторы удаляемых музыкальных жанров.</param>
    private Result RemoveGenres(Track track, IEnumerable<Guid> removingGenresIds)
    {
        var removingGenres = removingGenresIds.Select(GenreId.Create)
            .Select(albumId => albumId).ToList();

        var removeGenresResult = track.RemoveGenres(removingGenres);
        if (removeGenresResult.IsFailed)
        {
            return removeGenresResult;
        }

        return Result.Ok();
    }
}