namespace RftmAPI.Domain.Exceptions.TrackExceptions;

/// <summary>
/// Исключение даты выпуска доменной модели музыкального трека.
/// </summary>
public sealed class TrackReleaseDateException : TrackException
{
    /// <summary>
    /// Создание исключения даты выпуска доменной модели музыкального трека.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    public TrackReleaseDateException(string message) : base($"{nameof(TrackReleaseDateException)}. {message}")
    {
    }
}