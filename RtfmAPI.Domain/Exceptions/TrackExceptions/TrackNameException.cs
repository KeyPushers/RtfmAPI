namespace RftmAPI.Domain.Exceptions.TrackExceptions;

/// <summary>
/// Исключение названия доменной модели музыкального трека.
/// </summary>
public sealed class TrackNameException : TrackException
{
    /// <summary>
    /// Создание исключения названия доменной модели музыкального трека.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    public TrackNameException(string message) : base($"{nameof(TrackNameException)}. {message}")
    {
    }
}