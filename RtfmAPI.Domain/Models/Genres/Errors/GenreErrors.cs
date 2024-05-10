using FluentResults;

namespace RtfmAPI.Domain.Models.Genres.Errors;

internal static class GenreErrors
{
    #region GenreNameException

    internal static Error GenreNameIsNullOrEmpty() =>
        new("Не определено название музыкального жанра.");

    internal static Error GenreNameIsTooShort() =>
        new("Название музыкального жанра слишком короткое.");

    internal static Error GenreNameIsTooLong() =>
        new("Название музыкального жанра слишком длинное.");
    
    #endregion
}