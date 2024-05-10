using System.Threading.Tasks;
using FluentResults;
using RtfmAPI.Domain.Models.TrackFiles;
using RtfmAPI.Domain.Models.TrackFiles.ValueObjects;

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
    Task<Result<bool>> IsTrackFileExistsAsync(TrackFileId trackFileId);

    /// <summary>
    /// получение продолжительности файла музыкального трека.
    /// </summary>
    /// <param name="trackFileId">Идентифиактор файла музыкального трека.</param>
    /// <returns>Продолжительность файла музыкального трека.</returns>
    Task<Result<double>> GetTrackFileDurationAsync(TrackFileId trackFileId);
}