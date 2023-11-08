using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using RftmAPI.Domain.Exceptions.TrackExceptions;
using RftmAPI.Domain.Models.Albums.ValueObjects;
using RftmAPI.Domain.Models.Genres;
using RftmAPI.Domain.Models.Genres.ValueObjects;
using RftmAPI.Domain.Models.Tracks;
using RftmAPI.Domain.Models.Tracks.ValueObjects;
using RftmAPI.Domain.Primitives;
using RtfmAPI.Application.Common.Interfaces.Persistence;
using RtfmAPI.Application.Requests.Tracks.Queries.GetTrack.Dtos;

namespace RtfmAPI.Application.Requests.Tracks.Queries.GetTrack;

/// <summary>
/// Обработчик запроса информации о музыкальном треке.
/// </summary>
public class GetTrackQueryHandler : IRequestHandler<GetTrackQuery, Result<TrackInfo>>
{
    private readonly ITracksRepository _tracksRepository;
    private readonly IAlbumsRepository _albumsRepository;
    private readonly IGenresRepository _genresRepository;
    private readonly ILogger<GetTrackQueryHandler> _logger;

    /// <summary>
    /// Создание обработчика запроса информации о музыкальном треке.
    /// </summary>
    /// <param name="tracksRepository">Репозиторий музыкальных треков.</param>
    /// <param name="albumsRepository">Репозиторий музыкальных альбомов.</param>
    /// <param name="genresRepository">Репозиторий музыкальных жанров.</param>
    /// <param name="logger">Логгер.</param>
    public GetTrackQueryHandler(ITracksRepository tracksRepository, IAlbumsRepository albumsRepository,
        IGenresRepository genresRepository, ILogger<GetTrackQueryHandler> logger)
    {
        _tracksRepository = tracksRepository;
        _albumsRepository = albumsRepository;
        _genresRepository = genresRepository;
        _logger = logger;
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
        var track = await _tracksRepository.GetTrackByIdAsync(trackId);
        if (track is null)
        {
            return TrackExceptions.NotFound(trackId);
        }

        var albumTaskResult = GetAlbumOfTrackAsync(track);
        var genresTaskResult = GetGenresOfTrackAsync(track, cancellationToken);

        if (cancellationToken.IsCancellationRequested)
        {
            return new OperationCanceledException();
        }
        
        return new TrackInfo
        {
            Id = trackId.Value,
            Name = track.Name.Value,
            ReleaseDate = track.ReleaseDate.Value,
            Album = await albumTaskResult,
            Duration = track.Duration.Value,
            Genres = await genresTaskResult
        };
    }

    /// <summary>
    /// Получение информации об альбоме музыкального трека.
    /// </summary>
    /// <param name="track">Музыкальный трек.</param>
    /// <returns>Объект переноса данных информации об альбоме в музыкальном треке.</returns>
    private async Task<AlbumOfTrackInfo?> GetAlbumOfTrackAsync(Track track)
    {
        if (track.AlbumId is null)
        {
            return null;
        }

        var album = await _albumsRepository.GetAlbumByIdAsync(AlbumId.Create(track.AlbumId.Value));
        if (album is null)
        {
            return null;
        }
        
        return new AlbumOfTrackInfo
        {
            Id = album.Id.Value,
            Name = album.Name.Value
        };
    }

    /// <summary>
    /// Получение информации о жанрах музыкального трека.
    /// </summary>
    /// <param name="track"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Список жанров музыкального трека.</returns>
    private async Task<List<GenreOfTrackInfo>> GetGenresOfTrackAsync(Track track, CancellationToken cancellationToken = default)
    {
        if (!track.GenreIds.Any())
        {
            return new();
        }

        List<GenreOfTrackInfo> result = new();
        foreach (var genreId in track.GenreIds)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return new();
            }
            
            var genre = await _genresRepository.GetGenreByIdAsync(genreId);
            if (genre is null)
            {
                continue;
            }
            
            result.Add(new GenreOfTrackInfo
            {
                Id = genre.Id.Value,
                Name = genre.Name.Value
            });
        }

        return result;
    }
}