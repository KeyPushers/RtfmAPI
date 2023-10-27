namespace RftmAPI.Domain.Exceptions.BandExceptions;

/// <summary>
/// Исключение названия доменной модели музыкальной группы.
/// </summary>
public sealed class BandNameException : BandException
{
    /// <summary>
    /// Создание исключения названия доменной модели музыкальной группы.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    public BandNameException(string message) : base($"{nameof(BandNameException)}. {message}")
    {
    }
}