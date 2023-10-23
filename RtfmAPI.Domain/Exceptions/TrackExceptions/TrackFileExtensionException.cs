namespace RftmAPI.Domain.Exceptions.TrackExceptions;

/// <summary>
/// Исключение расширения файла доменной модели музыкального трека.
/// </summary>
public class TrackFileExtensionException : TrackException
{
    /// <summary>
    /// Создание исключения расширения файла доменной модели музыкального трека.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    public TrackFileExtensionException(string message) : base($"{nameof(TrackFileExtensionException)}.{message}")
    {
    }
}