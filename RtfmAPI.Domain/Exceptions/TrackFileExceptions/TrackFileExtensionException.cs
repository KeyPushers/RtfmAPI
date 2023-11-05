using RftmAPI.Domain.Exceptions.TrackExceptions;

namespace RftmAPI.Domain.Exceptions.TrackFileExceptions;

/// <summary>
/// Исключение расширения файла доменной модели музыкального трека.
/// </summary>
public sealed class TrackFileExtensionException : TrackException
{
    /// <summary>
    /// Создание исключения расширения файла доменной модели музыкального трека.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    public TrackFileExtensionException(string message) : base($"{nameof(TrackFileExtensionException)}. {message}")
    {
    }
}