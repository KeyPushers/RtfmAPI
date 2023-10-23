namespace RftmAPI.Domain.Exceptions.GenreExceptions;

/// <summary>
/// Исключение доменной модели музыкального жанра.
/// </summary>
public abstract class GenreException : DomainException
{
    /// <summary>
    /// Создание исключения доменной модели музыкального жанра.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    protected GenreException(string message) : base($"{nameof(GenreException)}.{message}")
    {
    }
}