using System.Linq;
using FluentResults;
using RtfmAPI.Application.Fabrics.Tracks.Daos;
using RtfmAPI.Domain.Models.Genres.ValueObjects;
using RtfmAPI.Domain.Models.TrackFiles.ValueObjects;
using RtfmAPI.Domain.Models.Tracks;
using RtfmAPI.Domain.Models.Tracks.ValueObjects;

namespace RtfmAPI.Application.Fabrics.Tracks;

/// <summary>
/// Фабрика музыкальных треков.
/// </summary>
public class TracksFactory : IEntityFactory<Track, TrackId>
{
    private readonly TrackDao _trackDao;

    /// <summary>
    /// Создание фабрики музыкальных треков.
    /// </summary>
    /// <param name="trackDao">Объект доступа данных музыкального трека.</param>
    public TracksFactory(TrackDao trackDao)
    {
        _trackDao = trackDao;
    }

    /// <inheritdoc />
    public Result<Track> Create()
    {
        var createTrackNameResult = TrackName.Create(_trackDao.Name ?? string.Empty);
        if (createTrackNameResult.IsFailed)
        {
            return createTrackNameResult.ToResult();
        }

        var trackName = createTrackNameResult.Value;

        var createTrackReleaseDateResult = TrackReleaseDate.Create(_trackDao.ReleaseDate);
        if (createTrackReleaseDateResult.IsFailed)
        {
            return createTrackReleaseDateResult.ToResult();
        }

        var trackReleaseDate = createTrackReleaseDateResult.Value;

        var trackFileId = _trackDao.TrackFileId is null ? null : TrackFileId.Create(_trackDao.TrackFileId.Value);

        var genreIds = _trackDao.GenreIds.Select(GenreId.Create);

        return Track.Create(trackName, trackReleaseDate, trackFileId, genreIds);
    }

    /// <inheritdoc />
    public Result<Track> Restore()
    {
        var trackId = TrackId.Create(_trackDao.Id);

        var createTrackNameResult = TrackName.Create(_trackDao.Name ?? string.Empty);
        if (createTrackNameResult.IsFailed)
        {
            return createTrackNameResult.ToResult();
        }

        var trackName = createTrackNameResult.Value;

        var createTrackReleaseDateResult = TrackReleaseDate.Create(_trackDao.ReleaseDate);
        if (createTrackReleaseDateResult.IsFailed)
        {
            return createTrackReleaseDateResult.ToResult();
        }

        var trackReleaseDate = createTrackReleaseDateResult.Value;

        var trackFileId = _trackDao.TrackFileId is null ? null : TrackFileId.Create(_trackDao.TrackFileId.Value);

        var genreIds = _trackDao.GenreIds.Select(GenreId.Create);

        return Track.Restore(trackId, trackName, trackReleaseDate, trackFileId, genreIds);
    }
}