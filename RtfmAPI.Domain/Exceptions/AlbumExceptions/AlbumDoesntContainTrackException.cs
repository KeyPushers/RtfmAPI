namespace RftmAPI.Domain.Exceptions.AlbumExceptions;

/// <summary>
/// Исключение отсутствия музыкального трека в доменной модели музыкального альбома.
/// </summary>
public sealed class AlbumDoesntContainTrackException : AlbumException
{
    /// <summary>
    /// Создание исключения отсутствия музыкального треека в доменной модели музыкального альбома.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    public AlbumDoesntContainTrackException(string message) : base($"{nameof(AlbumDoesntContainTrackException)}. {message}.")
    {
    }
}