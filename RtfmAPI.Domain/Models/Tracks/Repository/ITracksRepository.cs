using RftmAPI.Domain.Models.Tracks.ValueObjects;

namespace RftmAPI.Domain.Models.Tracks.Repository;

/// <summary>
/// Интерфейс репозитория доменной модели <see cref="Track"/>.
/// </summary>
public interface ITracksRepository
{
    /// <summary>
    /// Получение музыкальных треков.
    /// </summary>
    /// <returns>Список музыкальных треков.</returns>
    Task<List<Track>> GetTracksAsync();

    /// <summary>
    /// Получение музыкального трека по идентификатору.
    /// </summary>
    /// <param name="trackId">Идентифиактор музыкального трека.</param>
    /// <returns>Музыкальный трек.</returns>
    Task<Track?> GetTrackByIdAsync(TrackId trackId);
    
    /// <summary>
    /// Добавление музыкального трека.
    /// </summary>
    /// <param name="track">Музыкальный трек.</param>
    Task AddAsync(Track track);
}