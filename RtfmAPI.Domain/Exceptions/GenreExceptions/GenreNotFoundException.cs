namespace RftmAPI.Domain.Exceptions.GenreExceptions;

/// <summary>
/// Исключение неудачного поиска музыкального альбома.
/// </summary>
public class GenreNotFoundException : GenreException
{
    /// <summary>
    /// Создание исключения неудачного поиска музыкального жанра.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    public GenreNotFoundException(string message) : base($"{nameof(GenreNotFoundException)}. {message}")
    {
    }
}