using System;
using RtfmAPI.Domain.Models.Albums;
using RtfmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Application.Fabrics;

/// <summary>
/// Фабрика музыкальных альбомов.
/// </summary>
public class AlbumsFabric
{
    /// <summary>
    /// Создание музыкального альбома.
    /// </summary>
    /// <param name="name">Название музыкального альбома.</param>
    /// <param name="releaseDate">Дата выпуска.</param>
    /// <returns>Музыкальная группа.</returns>
    public Result<Album> CreateAlbum(string name, DateTime releaseDate)
    {
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
        
        return Album.Create(getAlbumNameResult.Value, getReleaseDateResult.Value);
    }
}