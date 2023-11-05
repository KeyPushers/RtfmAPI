using RftmAPI.Domain.Exceptions.TrackExceptions;

namespace RftmAPI.Domain.Exceptions.TrackFileExceptions;

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