namespace RftmAPI.Domain.Exceptions.TrackFileExceptions;

/// <summary>
/// Исключение доменной модели файла музыкального трека.
/// </summary>
public abstract class TrackFileException : DomainException
{
    /// <summary>
    /// Создание исключения доменной модели файла музыкального трека.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    protected TrackFileException(string message) : base($"{nameof(TrackFileException)}.{message}")
    {
    }
}