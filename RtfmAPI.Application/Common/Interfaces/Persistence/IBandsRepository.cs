using System.Collections.Generic;
using System.Threading.Tasks;
using RftmAPI.Domain.Models.Bands;
using RftmAPI.Domain.Models.Bands.ValueObjects;

namespace RtfmAPI.Application.Common.Interfaces.Persistence;

/// <summary>
/// Интерфейс репозитория доменной модели <see cref="Band"/>.
/// </summary>
public interface IBandsRepository
{
    /// <summary>
    /// Получение музыкальных групп.
    /// </summary>
    /// <returns>Список музыкальных групп.</returns>
    Task<List<Band>> GetBandsAsync();

    /// <summary>
    /// Получение музыкальной группы по идентификатору.
    /// </summary>
    /// <param name="bandId">Идентифиактор музыкальной группы.</param>
    /// <returns>Музыкальная группа.</returns>
    Task<Band?> GetBandByIdAsync(BandId bandId);
    
    /// <summary>
    /// Добавление музыкальной группы.
    /// </summary>
    /// <param name="band">Музыкальная группа.</param>
    Task AddAsync(Band band);
}