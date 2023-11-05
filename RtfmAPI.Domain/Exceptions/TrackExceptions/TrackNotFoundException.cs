namespace RftmAPI.Domain.Exceptions.TrackExceptions;

/// <summary>
/// Исключение неудачного поиска музыкального трека.
/// </summary>
public class TrackNotFoundException : TrackException
{
    /// <summary>
    /// Создание исключения неудачного поиска музыкального трека.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    public TrackNotFoundException(string message) : base($"{nameof(TrackNotFoundException)}. {message}")
    {
    }
}