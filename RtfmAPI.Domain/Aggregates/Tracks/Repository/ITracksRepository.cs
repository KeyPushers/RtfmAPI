namespace RftmAPI.Domain.Aggregates.Tracks.Repository;

/// <summary>
/// Интерфейс репозитория музыкальных треков
/// </summary>
public interface ITracksRepository
{
    /// <summary>
    /// Получение музыкальных треков
    /// </summary>
    /// <returns>Список музыкальных треков</returns>
    Task<List<Track>> GetTracksAsync();

    /// <summary>
    /// Получение музыкального трека по идентификатору
    /// </summary>
    /// <param name="id">Идентифиактор</param>
    /// <returns>Музыкальный трек</returns>
    Task<Track?> GetTrackByIdAsync(Guid id);
    
    /// <summary>
    /// Добавление музыкального трека
    /// </summary>
    /// <param name="track">Музыкальный трек</param>
    Task AddAsync(Track track);
}