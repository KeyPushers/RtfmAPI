using System.Threading.Tasks;
using RtfmAPI.Domain.Models.Genres;
using RtfmAPI.Domain.Models.Genres.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Application.Interfaces.Persistence.Queries;

/// <summary>
/// Интерфейс репозитория запросов доменной модели <see cref="Genre"/>.
/// </summary>
public interface IGenresQueriesRepository
{
    /// <summary>
    /// Получение музыкального жанра по идентификатору.
    /// </summary>
    /// <param name="genreId">Идентифиактор музыкального жанра.</param>
    /// <returns>Музыкальный жанр.</returns>
    Task<Result<Genre>> GetGenreByIdAsync(GenreId genreId);
}