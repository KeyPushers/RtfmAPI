namespace RftmAPI.Domain.Exceptions.AlbumExceptions;

/// <summary>
/// Исключение названия доменной модели музыкального альбома.
/// </summary>
public sealed class AlbumNameException : AlbumException
{
    /// <summary>
    /// Создание исключения названия доменной модели музыкального альбома.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    public AlbumNameException(string message) : base($"{nameof(AlbumNameException)}. {message}")
    {
    }
}