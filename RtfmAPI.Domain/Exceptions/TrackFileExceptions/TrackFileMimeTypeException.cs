using RftmAPI.Domain.Exceptions.TrackExceptions;

namespace RftmAPI.Domain.Exceptions.TrackFileExceptions;

/// <summary>
/// Исключение MIME-типа файла доменной модели музыкального трека.
/// </summary>
public sealed class TrackFileMimeTypeException : TrackException
{
    /// <summary>
    /// Создание исключения MIME-типа файла доменной модели музыкального трека.
    /// </summary>
    /// <param name="message">Сообщение</param>
    public TrackFileMimeTypeException(string message) : base($"{nameof(TrackFileMimeTypeException)}. {message}")
    {
    }
}