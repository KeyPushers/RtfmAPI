using RftmAPI.Domain.Models.Albums.ValueObjects;

namespace RftmAPI.Domain.Models.Albums.Repository;

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
    /// Добавление музыкального альбома.
    /// </summary>
    /// <param name="album">Музыкальный альбом.</param>
    Task AddAsync(Album album);
}