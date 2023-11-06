using RftmAPI.Domain.Exceptions.TrackExceptions;

namespace RftmAPI.Domain.Exceptions.TrackFileExceptions;

/// <summary>
/// Исключение продолжительности файла музыкального трека.
/// </summary>
public sealed class TrackFileDurationException : TrackException
{
    /// <summary>
    /// Создание исключения продолжительности файла музыкального трека.
    /// </summary>
    /// <param name="message">Сообщение</param>
    public TrackFileDurationException(string message) : base($"{nameof(TrackFileDurationException)}. {message}")
    {
    }
}