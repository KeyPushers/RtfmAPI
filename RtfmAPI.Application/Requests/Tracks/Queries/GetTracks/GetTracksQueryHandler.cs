﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using RftmAPI.Domain.Models.Albums;
using RftmAPI.Domain.Models.Albums.ValueObjects;
using RftmAPI.Domain.Models.Bands;
using RftmAPI.Domain.Models.Tracks;
using RftmAPI.Domain.Primitives;
using RtfmAPI.Application.Common.Interfaces.Persistence;
using RtfmAPI.Application.Requests.Tracks.Queries.GetTracks.Dtos;

namespace RtfmAPI.Application.Requests.Tracks.Queries.GetTracks;

/// <summary>
/// Обработчик запроса музыкальных треков
/// </summary>
public class GetTracksQueryHandler : IRequestHandler<GetTracksQuery, Result<TrackItems>>
{
    private readonly ITracksRepository _tracksRepository;
    private readonly IAlbumsRepository _albumsRepository;
    private readonly IBandsRepository _bandsRepository;
    private readonly ILogger<GetTracksQueryHandler> _logger;

    /// <summary>
    /// Обработчик запроса музыкальных треков
    /// </summary>
    /// <param name="tracksRepository">Репозиторий музыкальных треков.</param>
    /// <param name="albumsRepository">Репозиторий музыкальных альбомов.</param>
    /// <param name="bandsRepository">Репозиторий музыкальных групп.</param>
    /// <param name="logger">Логгер.</param>
    public GetTracksQueryHandler(ITracksRepository tracksRepository, IAlbumsRepository albumsRepository,
        IBandsRepository bandsRepository, ILogger<GetTracksQueryHandler> logger)
    {
        _tracksRepository = tracksRepository;
        _albumsRepository = albumsRepository;
        _bandsRepository = bandsRepository;
        _logger = logger;
    }

    /// <summary>
    /// Обработка запроса музыкальных треков
    /// </summary>
    /// <param name="request">Запрос музыкальных треков</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Музыкальные треки</returns>
    public async Task<Result<TrackItems>> Handle(GetTracksQuery request, CancellationToken cancellationToken = default)
    {
        var tracks = await _tracksRepository.GetTracksAsync().ConfigureAwait(false);

        var trackItems = await GetTrackItemsFromTracksAsync(tracks, cancellationToken);

        return new TrackItems
        {
            Tracks = trackItems
        };
    }

    /// <summary>
    /// Получение объектов переноса данных треков.
    /// </summary>
    /// <param name="tracks">Музыкальные треки.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список объектов переноса данных треков.</returns>
    private async Task<List<TrackItem>> GetTrackItemsFromTracksAsync(ICollection<Track> tracks,
        CancellationToken cancellationToken = default)
    {
        List<TrackItem> trackItems = new();
        foreach (var track in tracks)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return trackItems;
            }

            trackItems.Add(await GetTrackItemFromTrackAsync(track));
        }

        return trackItems;
    }

    /// <summary>
    /// Получение объекта переноса данных трека.
    /// </summary>
    /// <param name="track">Музыкальный трек.</param>
    /// <returns>Список объектов переноса данных треков.</returns>
    private async Task<TrackItem> GetTrackItemFromTrackAsync(Track track)
    {
        return new TrackItem
        {
            Id = track.Id.Value,
            Name = track.Name.Value,
            BandNames = await GetBandNamesAsync(track),
            Duration = track.Duration.Value
        };
    }

    /// <summary>
    /// Получение альбома по треку.
    /// </summary>
    /// <param name="track">Музыкальный трек.</param>
    /// <returns>Музыкальный альбом.</returns>
    private Task<Album?> GetAlbumByTrackAsync(Track track)
    {
        if (track.AlbumId is null)
        {
            return Task.FromResult<Album?>(null);
        }

        return _albumsRepository.GetAlbumByIdAsync(track.AlbumId);
    }

    /// <summary>
    /// Получение названий музыкальных групп, котоыре исполнили музыкальный трек.
    /// </summary>
    /// <param name="track">Музыкальный трек.</param>
    /// <returns>Список названий музыкальных групп.</returns>
    private async Task<List<string>> GetBandNamesAsync(Track track)
    {
        var album = await GetAlbumByTrackAsync(track);
        if (album is null)
        {
            return new();
        }

        var bands =  await GetBandsByAlbumAsync(album);
        return bands.Select(entity => entity.Name.Value).ToList();
    }
    
    /// <summary>
    /// Получение группы по альбому.
    /// </summary>
    /// <param name="album">Музыкальный альбом.</param>
    /// <returns>Музыкальная группа.</returns>
    private Task<List<Band>> GetBandsByAlbumAsync(Album album)
    {
        return _bandsRepository.GetBandsByAlbumIdAsync(AlbumId.Create(album.Id.Value));
    }
}