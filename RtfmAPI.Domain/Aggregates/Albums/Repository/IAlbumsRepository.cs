using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Aggregates.Albums.Repository;

/// <summary>
/// Интерфейс репозитория альбома
/// </summary>
public interface IAlbumsRepository
{
    /// <summary>
    /// Получение альбомов
    /// </summary>
    /// <returns>Список альбомов</returns>
    Task<List<Album>> GetAlbumsAsync();

    /// <summary>
    /// Получение альбома по идентификатору
    /// </summary>
    /// <param name="id">Идентифиактор</param>
    /// <returns>Альбом</returns>
    Task<Album?> GetAlbumByIdAsync(Guid id);
    
    /// <summary>
    /// Добавление альбома
    /// </summary>
    /// <param name="album">Альбом</param>
    Task AddAsync(Album album);
}