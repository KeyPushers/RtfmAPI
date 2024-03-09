using System.Threading.Tasks;
using RtfmAPI.Domain.Models.TrackFiles;
using RtfmAPI.Domain.Models.TrackFiles.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Application.Interfaces.Persistence.Queries;

/// <summary>
/// Интерфейс репозитория запросов доменной модели <see cref="TrackFile"/>.
/// </summary>
public interface ITrackFilesQueriesRepository
{
    /// <summary>
    /// Получение файла музыкального трека по идентификатору.
    /// </summary>
    /// <param name="trackFileId">Идентифиактор файла музыкального трека.</param>
    /// <returns>Музыкальный трек.</returns>
    Task<Result<TrackFile>> GetTrackFileByIdAsync(TrackFileId trackFileId);

    /// <summary>
    /// Получение признака существования файла музыкального трека.
    /// </summary>
    /// <param name="trackFileId">Идентифиактор файла музыкального трека.</param>
    Task<bool> IsTrackFileExistsAsync(TrackFileId trackFileId);
}