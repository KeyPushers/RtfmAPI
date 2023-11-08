namespace RftmAPI.Domain.Exceptions.TrackFileExceptions;

/// <summary>
/// Исключение неудачного поиска файла музыкального трека.
/// </summary>
public class TrackFileNotFoundException : TrackFileException
{
    /// <summary>
    /// Создание исключения неудачного поиска музыкального трека.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    public TrackFileNotFoundException(string message) : base($"{nameof(TrackFileNotFoundException)}. {message}")
    {
    }
}