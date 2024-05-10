using System.Linq;
using FluentResults;
using RtfmAPI.Application.Fabrics.Albums.Dao;
using RtfmAPI.Domain.Models.Albums;
using RtfmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Domain.Models.Tracks.ValueObjects;

namespace RtfmAPI.Application.Fabrics.Albums;

/// <summary>
/// Фабрика музыкальных альбомов.
/// </summary>
public class AlbumsFactory : IEntityFactory<Album, AlbumId>
{
    private readonly AlbumDao _albumDao;

    /// <summary>
    /// Создание фабрики музыкальных альбомов.
    /// </summary>
    /// <param name="albumDao">Объект доступа данных музыкального альбома.</param>
    public AlbumsFactory(AlbumDao albumDao)
    {
        _albumDao = albumDao;
    }

    /// <inheritdoc/>
    public Result<Album> Create()
    {
        var getAlbumNameResult = AlbumName.Create(_albumDao.Name ?? string.Empty);
        if (getAlbumNameResult.IsFailed)
        {
            return getAlbumNameResult.ToResult();
        }

        var albumName = getAlbumNameResult.ValueOrDefault;

        var getReleaseDateResult = AlbumReleaseDate.Create(_albumDao.ReleaseDate);
        if (getReleaseDateResult.IsFailed)
        {
            return getReleaseDateResult.ToResult();
        }

        var releaseDate = getReleaseDateResult.ValueOrDefault;

        var trackIds = _albumDao.TrackIds.Select(TrackId.Create);

        return Album.Create(albumName, releaseDate, trackIds);
    }

    /// <inheritdoc/>
    public Result<Album> Restore()
    {
        var albumId = AlbumId.Create(_albumDao.Id);

        var getAlbumNameResult = AlbumName.Create(_albumDao.Name ?? string.Empty);
        if (getAlbumNameResult.IsFailed)
        {
            return getAlbumNameResult.ToResult();
        }

        var albumName = getAlbumNameResult.ValueOrDefault;

        var getReleaseDateResult = AlbumReleaseDate.Create(_albumDao.ReleaseDate);
        if (getReleaseDateResult.IsFailed)
        {
            return getReleaseDateResult.ToResult();
        }

        var releaseDate = getReleaseDateResult.ValueOrDefault;

        var trackIds = _albumDao.TrackIds.Select(TrackId.Create);

        return Album.Restore(albumId, albumName, releaseDate, trackIds);
    }
}