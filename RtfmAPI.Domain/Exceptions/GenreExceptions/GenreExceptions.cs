using RftmAPI.Domain.Models.Genres.ValueObjects;

namespace RftmAPI.Domain.Exceptions.GenreExceptions;

/// <summary>
/// Исключения доменной модели музыкального жанра.
/// </summary>
public static class GenreExceptions
{
    /// <summary>
    /// Создание "<inheritdoc cref="GenreNotFoundException"/>".
    /// </summary>
    /// <param name="genreId">Идентификатор музыкального жанра.</param>
    public static GenreNotFoundException NotFound(GenreId genreId) =>
        new($"Не удалось найти музыкальный жанр с идентификатором [{genreId.Value}].");
    
    /// <summary>
    /// Исключения названия доменной модели музыкального жанра.
    /// </summary>
    public static class GenreNameExceptions
    {
        /// <inheritdoc cref="GenreNameException"/>
        public static GenreNameException IsNullOrWhiteSpace => new("Название музыкального жанра не задано.");
        
        /// <inheritdoc cref="GenreNameException"/>
        public static GenreNameException IsTooShort => new("Название музыкального жанра слишком короткое.");
        
        /// <inheritdoc cref="GenreNameException"/>
        public static GenreNameException IsTooLong => new("Название музыкального жанра слишком длинное.");
    }
}