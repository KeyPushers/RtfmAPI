using System.Threading.Tasks;
using RtfmAPI.Domain.Models.Albums;
using RtfmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Domain.Primitives;

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
}