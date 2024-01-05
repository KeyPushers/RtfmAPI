namespace RftmAPI.Domain.Exceptions.AlbumExceptions;

/// <summary>
/// Исключение добавления музыкального альбома.
/// </summary>
public sealed class AddAlbumException : AlbumException
{
    /// <summary>
    /// Создание исключения добавления музыкального альбома.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    public AddAlbumException(string message) : base($"{nameof(AlbumException)}.{message}")
    {
    }
}