namespace RftmAPI.Domain.Exceptions.AlbumExceptions;

/// <summary>
/// Исключение удаления музыкального трека из музыкального альбома.
/// </summary>
public class TrackRemovingFromAlbumFailedException : AlbumException
{
    /// <summary>
    /// Создание исключения удаления музыкального трека из музыкального альбома.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    public TrackRemovingFromAlbumFailedException(string message) : base(
        $"{nameof(TrackRemovingFromAlbumFailedException)}. {message}")
    {
    }
}