using System;
using RtfmAPI.Domain.Fabrics;
using RtfmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Albums;

/// <summary>
/// Фабрика музыкальных альбомов.
/// </summary>
public class AlbumsFabric : AggregateFabric<Album, AlbumId, Guid>
{
    private readonly string _name;
    private readonly DateTime _releaseDate;

    /// <summary>
    /// Создание фабрики музыкальных альбомов.
    /// </summary>
    /// <param name="name">название музыкального альбома.</param>
    /// <param name="releaseDate">Дата выпуска музыкального альбома.</param>
    public AlbumsFabric(string name, DateTime releaseDate)
    {
        _name = name;
        _releaseDate = releaseDate;
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

        return Album.Create(getAlbumNameResult.Value, getReleaseDateResult.Value);
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

        return Album.Restore(albumId, getAlbumNameResult.Value, getReleaseDateResult.Value);
    }
}