namespace RftmAPI.Domain.Exceptions.TrackExceptions;

/// <summary>
/// Исключение доменной модели музыкального трека.
/// </summary>
public sealed class AlbumIsAlreadyAddedToTrackException : TrackException
{
    /// <summary>
    /// Создание исключения доменной модели музыкального трека.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    public AlbumIsAlreadyAddedToTrackException(string message) : base($"{nameof(AlbumIsAlreadyAddedToTrackException)}. {message}")
    {
    }
}