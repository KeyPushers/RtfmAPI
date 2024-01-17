namespace RftmAPI.Domain.Exceptions.AlbumExceptions;

/// <summary>
/// Исключения удаления музыкального альбома.
/// </summary>
public class DeleteAlbumException : AlbumException
{
    /// <summary>
    /// Создание исключения удаления музыкального альбома.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    public DeleteAlbumException(string message) : base($"{nameof(DeleteAlbumException)}. {message}")
    {
    }
}