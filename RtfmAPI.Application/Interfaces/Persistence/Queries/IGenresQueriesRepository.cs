using System.Threading.Tasks;
using FluentResults;
using RtfmAPI.Domain.Models.Genres;
using RtfmAPI.Domain.Models.Genres.ValueObjects;

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
    
    /// <summary>
    /// Получение признака существования музыкального жанра.
    /// </summary>
    /// <param name="genreId">Идентификатор музыкального жанра.</param>
    Task<Result<bool>> IsGenreExistsAsync(GenreId genreId);
}