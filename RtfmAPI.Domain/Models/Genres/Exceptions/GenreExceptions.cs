namespace RtfmAPI.Domain.Models.Genres.Exceptions;

internal static class GenreExceptions
{
    #region GenreNameException

    internal static GenreNameException GenreNameIsNullOrEmpty() =>
        new("Не определено название музыкального жанра.");

    internal static GenreNameException GenreNameIsTooShort() =>
        new("Название музыкального жанра слишком короткое.");

    internal static GenreNameException GenreNameIsTooLong() =>
        new("Название музыкального жанра слишком длинное.");
    
    #endregion
}