namespace RftmAPI.Domain.Exceptions.AlbumExceptions;

/// <summary>
/// Исключение неудачного поиска музыкального альбома.
/// </summary>
public class AlbumNotFoundException : AlbumException
{
    /// <summary>
    /// Создание исключения неудачного поиска музыкального альбома.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    public AlbumNotFoundException(string message) : base($"{nameof(AlbumNotFoundException)}. {message}")
    {
    }
}