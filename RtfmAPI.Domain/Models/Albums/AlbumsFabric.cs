using System;
using RtfmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Albums;

/// <summary>
/// Фабрика музыкальных альбомов.
/// </summary>
public class AlbumsFabric
{
    /// <summary>
    /// Создание музыкального альбома.
    /// </summary>
    /// <param name="id">Идентификатор музыкального альбома.</param>
    /// <param name="name">Название музыкального альбома.</param>
    /// <param name="releaseDate">Дата выпуска.</param>
    /// <returns>Музыкальная группа.</returns>
    public Result<Album> Restore(Guid id, string name, DateTime releaseDate)
    {
        var albumId = AlbumId.Create(id);
        
        var getAlbumNameResult = AlbumName.Create(name);
        if (getAlbumNameResult.IsFailed)
        {
            return getAlbumNameResult.Error;
        }
        
        var getReleaseDateResult = AlbumReleaseDate.Create(releaseDate);
        if (getReleaseDateResult.IsFailed)
        {
            return getReleaseDateResult.Error;
        }
        
        return Album.Restore(albumId, getAlbumNameResult.Value, getReleaseDateResult.Value);
    }
}