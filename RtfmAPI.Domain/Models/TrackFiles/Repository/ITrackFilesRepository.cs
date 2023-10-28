using RftmAPI.Domain.Models.TrackFiles.ValueObjects;

namespace RftmAPI.Domain.Models.TrackFiles.Repository;

/// <summary>
/// Интерфейс репозитория доменной модели <see cref="TrackFile"/>.
/// </summary>
public interface ITrackFilesRepository
{
    /// <summary>
    /// Получение файла музыкального трека по идентификатору.
    /// </summary>
    /// <param name="trackFileId">Идентифиактор файла музыкального трека.</param>
    /// <returns>Файл музыкальный трека.</returns>
    Task<TrackFile?> GetTrackFileByIdAsync(TrackFileId trackFileId);
    
    /// <summary>
    /// Добавление файла музыкального трека.
    /// </summary>
    /// <param name="trackFile">Файл музыкального трека.</param>
    Task AddAsync(TrackFile trackFile);
}