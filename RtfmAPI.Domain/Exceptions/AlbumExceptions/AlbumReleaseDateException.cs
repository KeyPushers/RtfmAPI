namespace RftmAPI.Domain.Exceptions.AlbumExceptions;

/// <summary>
/// Исключение даты выпуска доменной модели музыкального альбома.
/// </summary>
public sealed class AlbumReleaseDateException : AlbumException
{
    /// <summary>
    /// Создание исключения даты выпуска доменной модели музыкального альбома.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    public AlbumReleaseDateException(string message) : base($"{nameof(AlbumReleaseDateException)}. {message}")
    {
    }
}