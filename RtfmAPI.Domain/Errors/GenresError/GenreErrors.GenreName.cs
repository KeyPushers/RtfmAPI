using RftmAPI.Domain.Shared;

namespace RftmAPI.Domain.Errors.GenresError;

/// <summary>
/// Ошибки доменной модели музыкального жанра.
/// </summary>
public static partial class GenreErrors
{
    /// <summary>
    /// Ошибки названия музыкальной группы.
    /// </summary>
    public static class GenreName
    {
        /// <summary>
        /// Ошибка незаданного наименования музыкальног
        /// </summary>
        public static readonly Error IsNullOrWhiteSpace = new(
            $"{nameof(GenreErrors)}.{nameof(GenreName)}.{nameof(IsNullOrWhiteSpace)}",
            "Название музыкального жанра не задано.");

        /// <summary>
        /// Ошибка короткого наименования музыкального альбома.
        /// </summary>
        public static readonly Error IsTooShort = new(
            $"{nameof(GenreErrors)}.{nameof(GenreName)}.{nameof(IsTooShort)}",
            "Название музыкального жанра слишком короткое.");

        /// <summary>
        /// Ошибка длинного наименования альбома.
        /// </summary>
        public static readonly Error IsTooLong = new(
            $"{nameof(GenreErrors)}.{nameof(GenreName)}.{nameof(IsTooLong)}",
            "Название музыкального жанра слишком длинное.");
    }
}