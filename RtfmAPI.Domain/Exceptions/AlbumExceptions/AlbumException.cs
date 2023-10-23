namespace RftmAPI.Domain.Exceptions.AlbumExceptions;

/// <summary>
/// Исключение доменной модели музыкального альбома.
/// </summary>
public abstract class AlbumException : DomainException
{
    /// <summary>
    /// Создание исключения доменной модели музыкуального альбома.
    /// </summary>
    /// <param name="message">Сообющение.</param>
    protected AlbumException(string message) : base($"{nameof(AlbumException)}.{message}")
    {
    }
}