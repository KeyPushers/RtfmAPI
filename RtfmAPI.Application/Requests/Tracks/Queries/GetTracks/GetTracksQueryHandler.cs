using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using MediatR;
using RtfmAPI.Application.Interfaces.Persistence.Queries;
using RtfmAPI.Application.Requests.Tracks.Queries.GetTracks.Dtos;
using RtfmAPI.Domain.Models.Bands;
using RtfmAPI.Domain.Models.Tracks;

namespace RtfmAPI.Application.Requests.Tracks.Queries.GetTracks;

/// <summary>
/// Обработчик запроса музыкальных треков.
/// </summary>
public class GetTracksQueryHandler : IRequestHandler<GetTracksQuery, Result<TrackItems>>
{
    private readonly ITracksQueriesRepository _tracksQueriesRepository;
    private readonly ITrackFilesQueriesRepository _trackFilesQueriesRepository;
    private readonly IAlbumsQueriesRepository _albumsQueriesRepository;
    private readonly IBandsQueriesRepository _bandsQueriesRepository;

    /// <summary>
    /// Обработчик запроса музыкальных треков.
    /// </summary>
    /// <param name="tracksQueriesRepository">Репозиторий запросов музыкальных треков.</param>
    /// <param name="trackFilesQueriesRepository">Репозиторий запросов файлов музыкальных треков.</param>
    /// <param name="albumsQueriesRepository">Репозиторий запросов музыкальных альбомов.</param>
    /// <param name="bandsQueriesRepository">Репозиторий запросов музыкальных групп.</param>
    public GetTracksQueryHandler(ITracksQueriesRepository tracksQueriesRepository,
        ITrackFilesQueriesRepository trackFilesQueriesRepository, IAlbumsQueriesRepository albumsQueriesRepository,
        IBandsQueriesRepository bandsQueriesRepository)
    {
        _tracksQueriesRepository = tracksQueriesRepository;
        _trackFilesQueriesRepository = trackFilesQueriesRepository;
        _albumsQueriesRepository = albumsQueriesRepository;
        _bandsQueriesRepository = bandsQueriesRepository;
    }

    /// <summary>
    /// Обработка запроса музыкальных треков
    /// </summary>
    /// <param name="request">Запрос музыкальных треков</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Музыкальные треки</returns>
    public async Task<Result<TrackItems>> Handle(GetTracksQuery request, CancellationToken cancellationToken = default)
    {
        var getTracksResult = await _tracksQueriesRepository.GetTracksAsync(cancellationToken);
        if (getTracksResult.IsFailed)
        {
            return getTracksResult.ToResult();
        }

        var tracks = getTracksResult.ValueOrDefault;

        var getTrackItemsResult = await GetTrackItemsFromTracksAsync(tracks, cancellationToken);
        if (getTrackItemsResult.IsFailed)
        {
            return getTrackItemsResult.ToResult();
        }

        return new TrackItems
        {
            Tracks = getTrackItemsResult.ValueOrDefault
        };
    }

    /// <summary>
    /// Получение объектов переноса данных треков.
    /// </summary>
    /// <param name="tracks">Музыкальные треки.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список объектов переноса данных треков.</returns>
    private async Task<Result<List<TrackItem>>> GetTrackItemsFromTracksAsync(IEnumerable<Track> tracks,
        CancellationToken cancellationToken = default)
    {
        List<TrackItem> trackItems = new();
        foreach (var track in tracks)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var getTrackItemResult = await GetTrackItemFromTrackAsync(track);
            if (getTrackItemResult.IsFailed)
            {
                return getTrackItemResult.ToResult();
            }

            trackItems.Add(getTrackItemResult.ValueOrDefault);
        }

        return trackItems;
    }

    /// <summary>
    /// Получение объекта переноса данных трека.
    /// </summary>
    /// <param name="track">Музыкальный трек.</param>
    /// <returns>Список объектов переноса данных треков.</returns>
    private async Task<Result<TrackItem>> GetTrackItemFromTrackAsync(Track track)
    {
        if (track.TrackFileId is null)
        {
            throw new NotImplementedException();
        }

        var getDurationResult = await _trackFilesQueriesRepository.GetTrackFileDurationAsync(track.TrackFileId);
        if (getDurationResult.IsFailed)
        {
            return getDurationResult.ToResult();
        }

        return new TrackItem
        {
            Id = track.Id.Value,
            Name = track.Name.Value,
            BandNames = await GetBandNamesAsync(track),
            Duration = track.TrackFileId is null
                ? 0.0
                : getDurationResult.ValueOrDefault
        };
    }

    /// <summary>
    /// Получение названий музыкальных групп, котоыре исполнили музыкальный трек.
    /// </summary>
    /// <param name="track">Музыкальный трек.</param>
    /// <returns>Список названий музыкальных групп.</returns>
    private async Task<List<string>> GetBandNamesAsync(Track track)
    {
        var getAlbumsResult = await _albumsQueriesRepository.GetAlbumsByTrackIdAsync(track.Id);
        if (getAlbumsResult.IsFailed)
        {
            return new List<string>();
        }

        var albums = getAlbumsResult.ValueOrDefault;

        List<Band> result = new();
        foreach (var album in albums)
        {
            var getBandsResult = await _bandsQueriesRepository.GetBandsByAlbumIdAsync(album.Id);
            if (getBandsResult.IsFailed)
            {
                return new List<string>();
            }

            result.AddRange(getBandsResult.ValueOrDefault);
        }

        return result
            .DistinctBy(entity => entity.Id.Value)
            .Select(entity => entity.Name.Value).ToList();
    }
}