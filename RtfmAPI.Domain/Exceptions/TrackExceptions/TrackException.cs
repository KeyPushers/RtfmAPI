namespace RftmAPI.Domain.Exceptions.TrackExceptions;

/// <summary>
/// Исключение доменной модели музыкального трека.
/// </summary>
public abstract class TrackException : DomainException
{
    /// <summary>
    /// Создание исключения доменной модели музыкального трека.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    protected TrackException(string message) : base($"{nameof(TrackException)}.{message}")
    {
    }
}