using System.Threading.Tasks;
using RftmAPI.Domain.Models.TrackFiles;
using RftmAPI.Domain.Models.TrackFiles.ValueObjects;

namespace RtfmAPI.Application.Common.Interfaces.Persistence;

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

    /// <summary>
    /// Удаление файла музыкального трека.
    /// </summary>
    /// <param name="trackFile">Файл музыкального трека.</param>
    Task<bool> DeleteAsync(TrackFile trackFile);
}