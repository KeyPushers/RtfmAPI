using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RtfmAPI.Domain.Models.Tracks;
using RtfmAPI.Domain.Models.Tracks.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Application.Interfaces.Persistence.Queries;

/// <summary>
/// Интерфейс репозитория запросов доменной модели <see cref="Track"/>.
/// </summary>
public interface ITracksQueriesRepository
{
    /// <summary>
    /// Получение музыкального трека по идентификатору.
    /// </summary>
    /// <param name="trackId">Идентифиактор музыкального трека.</param>
    /// <returns>Музыкальный трек.</returns>
    Task<Result<Track>> GetTrackByIdAsync(TrackId trackId);

    /// <summary>
    /// Получение признака существования музыкального трека.
    /// </summary>
    /// <param name="trackId">Идентификатор музыкального трека.</param>
    Task<bool> IsTrackExistsAsync(TrackId trackId);

    /// <summary>
    /// Получение музыкальных треков.
    /// </summary>
    /// <returns>Музыкальные треки.</returns>
    Task<Result<List<Track>>> GetTracksAsync(CancellationToken cancellationToken = default);
}