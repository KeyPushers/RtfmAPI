using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using MediatR;
using RtfmAPI.Application.Interfaces.Persistence.Queries;
using RtfmAPI.Application.Requests.Tracks.Queries.GetTrack.Dtos;
using RtfmAPI.Domain.Models.TrackFiles.ValueObjects;
using RtfmAPI.Domain.Models.Tracks;
using RtfmAPI.Domain.Models.Tracks.ValueObjects;

namespace RtfmAPI.Application.Requests.Tracks.Queries.GetTrack;

/// <summary>
/// Обработчик запроса информации о музыкальном треке.
/// </summary>
public class GetTrackQueryHandler : IRequestHandler<GetTrackQuery, Result<TrackInfo>>
{
    private readonly ITracksQueriesRepository _tracksQueriesRepository;
    private readonly ITrackFilesQueriesRepository _trackFilesQueriesRepository;
    private readonly IAlbumsQueriesRepository _albumsQueriesRepository;
    private readonly IGenresQueriesRepository _genresQueriesRepository;

    /// <summary>
    /// Создание обработчика запроса информации о музыкальном треке.
    /// </summary>
    /// <param name="tracksQueriesRepository">Репозиторий запросов музыкальных треков.</param>
    /// <param name="trackFilesQueriesRepository">Репозиторий запросов файлов музыкальных треков.</param>
    /// <param name="albumsQueriesRepository">Репозиторий запросов музыкальных альбомов.</param>
    /// <param name="genresQueriesRepository">Репозиторий запросов музыкальных жанров.</param>
    public GetTrackQueryHandler(ITracksQueriesRepository tracksQueriesRepository,
        ITrackFilesQueriesRepository trackFilesQueriesRepository, IAlbumsQueriesRepository albumsQueriesRepository,
        IGenresQueriesRepository genresQueriesRepository)
    {
        _tracksQueriesRepository = tracksQueriesRepository;
        _trackFilesQueriesRepository = trackFilesQueriesRepository;
        _albumsQueriesRepository = albumsQueriesRepository;
        _genresQueriesRepository = genresQueriesRepository;
    }

    /// <summary>
    /// Обработка запроса информации о музыкальном треке.
    /// </summary>
    /// <param name="request">Запрос информации о музыкальном треке.</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Объект переноса данных информации о музыкальном треке.</returns>
    public async Task<Result<TrackInfo>> Handle(GetTrackQuery request, CancellationToken cancellationToken = default)
    {
        var trackId = TrackId.Create(request.Id);

        var getTrackResult = await _tracksQueriesRepository.GetTrackByIdAsync(trackId);
        if (getTrackResult.IsFailed)
        {
            return getTrackResult.ToResult();
        }

        var track = getTrackResult.Value;

        var getAlbumsResultTask = GetAlbumsOfTrackAsync(track, cancellationToken);
        var getGenresResultTask = GetGenresOfTrackAsync(track, cancellationToken);

        var getAlbumsResult = await getAlbumsResultTask;
        if (getAlbumsResult.IsFailed)
        {
            return getAlbumsResult.ToResult();
        }

        var albums = getAlbumsResult.ValueOrDefault;

        var getGenresResult = await getGenresResultTask;
        if (getGenresResult.IsFailed)
        {
            return getGenresResult.ToResult();
        }

        var genres = getGenresResult.ValueOrDefault;

        var duration = 0.0;
        if (track.TrackFileId is not null)
        {
            var getDurationResult = await GetTrackFileDurationAsync(track.TrackFileId);
            if (getDurationResult.IsFailed)
            {
                return getDurationResult.ToResult();
            }

            duration = getDurationResult.ValueOrDefault;
        }

        return new TrackInfo
        {
            Id = trackId.Value,
            Name = track.Name.Value,
            ReleaseDate = track.ReleaseDate.Value,
            Albums = albums,
            Duration = duration,
            Genres = genres
        };
    }

    /// <summary>
    /// Получение информации об альбоме музыкального трека.
    /// </summary>
    /// <param name="track">Музыкальный трек.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Объект переноса данных информации об альбоме в музыкальном треке.</returns>
    private async Task<Result<List<AlbumOfTrackInfo>>> GetAlbumsOfTrackAsync(Track track,
        CancellationToken cancellationToken = default)
    {
        var getAlbumsResult = await _albumsQueriesRepository.GetAlbumsByTrackIdAsync(track.Id, cancellationToken);
        if (getAlbumsResult.IsFailed)
        {
            return getAlbumsResult.ToResult();
        }

        var albums = getAlbumsResult.ValueOrDefault;

        List<AlbumOfTrackInfo> result = new();
        foreach (var album in albums)
        {
            cancellationToken.ThrowIfCancellationRequested();

            result.Add(new AlbumOfTrackInfo
            {
                Id = album.Id.Value,
                Name = album.Name.Value
            });
        }

        return result;
    }

    /// <summary>
    /// Получение информации о жанрах музыкального трека.
    /// </summary>
    /// <param name="track"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Список жанров музыкального трека.</returns>
    private async Task<Result<List<GenreOfTrackInfo>>> GetGenresOfTrackAsync(Track track,
        CancellationToken cancellationToken = default)
    {
        if (!track.GenreIds.Any())
        {
            return new List<GenreOfTrackInfo>();
        }

        List<GenreOfTrackInfo> result = new();
        foreach (var genreId in track.GenreIds)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var getGenreResult = await _genresQueriesRepository.GetGenreByIdAsync(genreId);
            if (getGenreResult.IsFailed)
            {
                return getGenreResult.ToResult();
            }

            var genre = getGenreResult.ValueOrDefault;

            result.Add(new GenreOfTrackInfo
            {
                Id = genre.Id.Value,
                Name = genre.Name.Value
            });
        }

        return result;
    }

    /// <summary>
    /// Получение продолжительности файла музыкального трека.
    /// </summary>
    /// <param name="trackFileId">Идентификатор файла музыкального трека.</param>
    private Task<Result<double>> GetTrackFileDurationAsync(TrackFileId trackFileId)
    {
        return _trackFilesQueriesRepository.GetTrackFileDurationAsync(trackFileId);
    }
}