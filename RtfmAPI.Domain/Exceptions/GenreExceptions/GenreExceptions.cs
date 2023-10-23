namespace RftmAPI.Domain.Exceptions.GenreExceptions;

/// <summary>
/// Исключения доменной модели музыкального жанра.
/// </summary>
public static class GenreExceptions
{
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