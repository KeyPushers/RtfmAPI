namespace RftmAPI.Domain.Exceptions.TrackExceptions;

/// <summary>
/// Исключения удаления музыкального трека.
/// </summary>
public class DeleteTrackException : TrackException
{
    /// <summary>
    /// Создание исключения удаления музыкального трека.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    public DeleteTrackException(string message) : base($"{nameof(DeleteTrackException)}. {message}")
    {
    }
}