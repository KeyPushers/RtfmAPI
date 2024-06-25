using System;
using System.Collections.Generic;
using System.Linq;
using RtfmAPI.Domain.Fabrics;
using RtfmAPI.Domain.Models.Genres.ValueObjects;
using RtfmAPI.Domain.Models.TrackFiles.ValueObjects;
using RtfmAPI.Domain.Models.Tracks.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Tracks;

/// <summary>
/// Фабрика музыкальных треков.
/// </summary>
public class TracksFabric : AggregateFabric<Track, TrackId, Guid>
{
    private readonly string _name;
    private readonly DateTime _releaseDate;
    private readonly Guid? _trackFileId;
    private readonly IEnumerable<Guid> _genreIds;

    /// <summary>
    /// Созданеи фабрики музыкальных треков.
    /// </summary>
    /// <param name="name">Название музыкального трека.</param>
    /// <param name="releaseDate">Дата выпуска музыкального трека.</param>
    /// <param name="trackFileId">Идентификатор файла музыкального трека.</param>
    /// <param name="genreIds">Идентификаторы жанров музыкальных треков.</param>
    public TracksFabric(string name, DateTime releaseDate, Guid? trackFileId, IEnumerable<Guid> genreIds)
    {
        _name = name;
        _releaseDate = releaseDate;
        _trackFileId = trackFileId;
        _genreIds = genreIds;
    }

    /// <inheritdoc />
    public override Result<Track> Create()
    {
        var createTrackNameResult = TrackName.Create(_name);
        if (createTrackNameResult.IsFailed)
        {
            return createTrackNameResult.Error;
        }

        var trackName = createTrackNameResult.Value;

        var createTrackReleaseDateResult = TrackReleaseDate.Create(_releaseDate);
        if (createTrackReleaseDateResult.IsFailed)
        {
            return createTrackReleaseDateResult.Error;
        }

        var trackReleaseDate = createTrackReleaseDateResult.Value;

        var trackFileId = _trackFileId is null ? null : TrackFileId.Create(_trackFileId.Value);

        var genreIds = _genreIds.Select(GenreId.Create);

        return Track.Create(trackName, trackReleaseDate, trackFileId, genreIds);
    }

    /// <inheritdoc />
    public override Result<Track> Restore(Guid id)
    {
        var trackId = TrackId.Create(id);

        var createTrackNameResult = TrackName.Create(_name);
        if (createTrackNameResult.IsFailed)
        {
            return createTrackNameResult.Error;
        }

        var trackName = createTrackNameResult.Value;

        var createTrackReleaseDateResult = TrackReleaseDate.Create(_releaseDate);
        if (createTrackReleaseDateResult.IsFailed)
        {
            return createTrackReleaseDateResult.Error;
        }

        var trackReleaseDate = createTrackReleaseDateResult.Value;

        var trackFileId = _trackFileId is null ? null : TrackFileId.Create(_trackFileId.Value);

        var genreIds = _genreIds.Select(GenreId.Create);

        return Track.Restore(trackId, trackName, trackReleaseDate, trackFileId, genreIds);
    }
}