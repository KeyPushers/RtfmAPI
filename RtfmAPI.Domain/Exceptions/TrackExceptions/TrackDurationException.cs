namespace RftmAPI.Domain.Exceptions.TrackExceptions;

/// <summary>
/// Исключение продолжительности музыкального трека.
/// </summary>
public sealed class TrackDurationException : TrackException
{
    /// <summary>
    /// Создание исключения продолжительности музыкального трека.
    /// </summary>
    /// <param name="message">Сообщение</param>
    public TrackDurationException(string message) : base($"{nameof(TrackDurationException)}. {message}")
    {
    }
}