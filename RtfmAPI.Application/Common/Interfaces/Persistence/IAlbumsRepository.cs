using System.Collections.Generic;
using System.Threading.Tasks;
using RftmAPI.Domain.Models.Albums;
using RftmAPI.Domain.Models.Albums.ValueObjects;
using RftmAPI.Domain.Models.Tracks.ValueObjects;

namespace RtfmAPI.Application.Common.Interfaces.Persistence;

/// <summary>
/// Интерфейс репозитория доменной модели <see cref="Album"/>.
/// </summary>
public interface IAlbumsRepository
{
    /// <summary>
    /// Получение музыкальных альбомов.
    /// </summary>
    /// <returns>Список музыкальных альбомов.</returns>
    Task<List<Album>> GetAlbumsAsync();

    /// <summary>
    /// Получение музыкального альбома по идентификатору.
    /// </summary>
    /// <param name="albumId">Идентифиактор музыкального альбома.</param>
    /// <returns>Музыкальный альбом.</returns>
    Task<Album?> GetAlbumByIdAsync(AlbumId albumId);

    /// <summary>
    /// Получение музыкальных альбомов с музыкальным треком.
    /// </summary>
    /// <param name="trackId">Идентификатор музыкального трека.</param>
    /// <returns>Музыкальные альбомы.</returns>
    Task<List<Album>> GetAlbumsByTrackIdAsync(TrackId trackId);
    
    /// <summary>
    /// Добавление музыкального альбома.
    /// </summary>
    /// <param name="album">Музыкальный альбом.</param>
    Task AddAsync(Album album);

    /// <summary>
    /// Обновление музыкального альбома.
    /// </summary>
    /// <param name="album">Музыкальный альбом.</param>
    Task UpdateAsync(Album album);
    
    /// <summary>
    /// Удаление музыкального альбома.
    /// </summary>
    /// <param name="album">Музыкальный альбом.</param>
    Task<bool> DeleteAlbumAsync(Album album);
}