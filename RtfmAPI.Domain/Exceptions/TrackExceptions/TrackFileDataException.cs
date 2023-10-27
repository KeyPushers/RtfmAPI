namespace RftmAPI.Domain.Exceptions.TrackExceptions;

/// <summary>
/// Исключение содержимого файла доменной модели музыкального трека.
/// </summary>
public sealed class TrackFileDataException : TrackException
{
    /// <summary>
    /// Создание исключения содержимого файла доменной модели музыкального трека.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    public TrackFileDataException(string message) : base($"{nameof(TrackFileDataException)}. {message}")
    {
    }
}