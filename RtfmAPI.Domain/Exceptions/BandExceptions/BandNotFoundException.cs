namespace RftmAPI.Domain.Exceptions.BandExceptions;

/// <summary>
/// Исключение неудачного поиска музыкальной группы.
/// </summary>
public class BandNotFoundException : BandException
{
    /// <summary>
    /// Создание исключения неудачного поиска музыкального трека.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    public BandNotFoundException(string message) : base($"{nameof(BandNotFoundException)}. {message}")
    {
    }
}