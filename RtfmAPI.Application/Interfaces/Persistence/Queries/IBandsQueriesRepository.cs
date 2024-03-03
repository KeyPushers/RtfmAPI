using System.Threading.Tasks;
using RtfmAPI.Domain.Models.Bands;
using RtfmAPI.Domain.Models.Bands.ValueObjects;
using RtfmAPI.Domain.Primitives;

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
}