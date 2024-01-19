namespace RftmAPI.Domain.Exceptions.TrackFileExceptions;

/// <summary>
/// Исключения удаления файла музыкального трека.
/// </summary>
public class DeleteTrackFileException : TrackFileException
{
    /// <summary>
    /// Создание исключения удаления файла музыкального трека.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    public DeleteTrackFileException(string message) : base($"{nameof(DeleteTrackFileException)}. {message}")
    {
    }
}