using System;
using System.Collections.Generic;
using System.Linq;
using RtfmAPI.Domain.Fabrics;
using RtfmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Domain.Models.Tracks.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Albums;

/// <summary>
/// Фабрика музыкальных альбомов.
/// </summary>
public class AlbumsFabric : AggregateFabric<Album, AlbumId, Guid>
{
    private readonly string _name;
    private readonly DateTime _releaseDate;
    private readonly IEnumerable<Guid> _trackIds;

    /// <summary>
    /// Создание фабрики музыкальных альбомов.
    /// </summary>
    /// <param name="name">название музыкального альбома.</param>
    /// <param name="releaseDate">Дата выпуска музыкального альбома.</param>
    /// <param name="trackIds">Идентификаторы музыкальных треков.</param>
    public AlbumsFabric(string name, DateTime releaseDate, IEnumerable<Guid> trackIds)
    {
        _name = name;
        _releaseDate = releaseDate;
        _trackIds = trackIds;
    }
    
    /// <inheritdoc />
    public override Result<Album> Create()
    {
        var getAlbumNameResult = AlbumName.Create(_name);
        if (getAlbumNameResult.IsFailed)
        {
            return getAlbumNameResult.Error;
        }

        var getReleaseDateResult = AlbumReleaseDate.Create(_releaseDate);
        if (getReleaseDateResult.IsFailed)
        {
            return getReleaseDateResult.Error;
        }

        var trackIds = _trackIds.Select(TrackId.Create);
        
        return Album.Create(getAlbumNameResult.Value, getReleaseDateResult.Value, trackIds);
    }

    /// <inheritdoc />
    public override Result<Album> Restore(Guid id)
    {
        var albumId = AlbumId.Create(id);

        var getAlbumNameResult = AlbumName.Create(_name);
        if (getAlbumNameResult.IsFailed)
        {
            return getAlbumNameResult.Error;
        }

        var getReleaseDateResult = AlbumReleaseDate.Create(_releaseDate);
        if (getReleaseDateResult.IsFailed)
        {
            return getReleaseDateResult.Error;
        }

        var trackIds = _trackIds.Select(TrackId.Create);

        return Album.Restore(albumId, getAlbumNameResult.Value, getReleaseDateResult.Value, trackIds);
    }
}