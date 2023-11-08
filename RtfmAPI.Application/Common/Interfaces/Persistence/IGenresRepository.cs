using System.Collections.Generic;
using System.Threading.Tasks;
using RftmAPI.Domain.Models.Genres;
using RftmAPI.Domain.Models.Genres.ValueObjects;

namespace RtfmAPI.Application.Common.Interfaces.Persistence;

/// <summary>
/// Интерфейс репозитория музыкальных жанров.
/// </summary>
public interface IGenresRepository
{
    /// <summary>
    /// Получение музыкальных жанров.
    /// </summary>
    /// <returns>Список музыкальных жанров.</returns>
    Task<List<Genre>> GetGenresAsync();
    
    /// <summary>
    /// Получение музыкального жанра по идентификатору.
    /// </summary>
    /// <param name="genreId">Идентификатор музыкального жанра.</param>
    /// <returns>Музыкальный жанр.</returns>
    Task<Genre?> GetGenreByIdAsync(GenreId genreId);
    
    /// <summary>
    /// Добавление музыкального жанра.
    /// </summary>
    /// <param name="genre">Музыкальный жанр.</param>
    Task AddAsync(Genre genre);
}