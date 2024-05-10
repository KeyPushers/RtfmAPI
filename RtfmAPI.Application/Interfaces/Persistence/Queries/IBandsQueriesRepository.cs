using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using RtfmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Domain.Models.Bands;
using RtfmAPI.Domain.Models.Bands.ValueObjects;

namespace RtfmAPI.Application.Interfaces.Persistence.Queries;

/// <summary>
/// Интерфейс репозитория запросов доменной модели <see cref="Band"/>.
/// </summary>
public interface IBandsQueriesRepository
{
    /// <summary>
    /// Получение музыкальной группы по идентификатору.
    /// </summary>
    /// <param name="bandId">Идентифиактор музыкальной группы.</param>
    /// <returns>Музыкальная группа.</returns>
    Task<Result<Band>> GetBandByIdAsync(BandId bandId);

    /// <summary>
    /// Получение музыкальных групп по музыкальому альбому.
    /// </summary>
    /// <param name="albumId">Идентификатор музыкального альбома.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Музыкальные группы.</returns>
    Task<Result<List<Band>>> GetBandsByAlbumIdAsync(AlbumId albumId, CancellationToken cancellationToken = default);
}