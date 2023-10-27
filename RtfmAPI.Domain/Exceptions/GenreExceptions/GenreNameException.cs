namespace RftmAPI.Domain.Exceptions.GenreExceptions;

/// <summary>
/// Исключение названия доменной модели музыкального жанра.
/// </summary>
public sealed class GenreNameException : GenreException
{
    /// <summary>
    /// Создание исключения названия доменной модели музыкального жанра.
    /// </summary>
    /// <param name="message">Сообщение</param>
    public GenreNameException(string message) : base($"{nameof(GenreNameException)}. {message}")
    {
    }
}