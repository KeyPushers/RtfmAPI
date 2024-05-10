using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using RtfmAPI.Domain.Models.Albums;
using RtfmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Domain.Models.Tracks.ValueObjects;

namespace RtfmAPI.Application.Interfaces.Persistence.Queries;

/// <summary>
/// Интерфейс репозитория запросов доменной модели <see cref="Album"/>.
/// </summary>
public interface IAlbumsQueriesRepository
{
    /// <summary>
    /// Получение музыкального альбома по идентификатору.
    /// </summary>
    /// <param name="albumId">Идентифиактор музыкального альбома.</param>
    /// <returns>Музыкальный альбом.</returns>
    Task<Result<Album>> GetAlbumByIdAsync(AlbumId albumId);

    /// <summary>
    /// Получение признака существования музыкального альбома.
    /// </summary>
    /// <param name="albumId">Идентификатор музыкального альбома.</param>
    Task<Result<bool>> IsAlbumExistsAsync(AlbumId albumId);

    /// <summary>
    /// Получение альбомов с музыкальным треком.
    /// </summary>
    /// <param name="trackId">Идентификатор музыкального трека.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Музыкальные альбомы.</returns>
    Task<Result<List<Album>>> GetAlbumsByTrackIdAsync(TrackId trackId, CancellationToken cancellationToken = default);
}