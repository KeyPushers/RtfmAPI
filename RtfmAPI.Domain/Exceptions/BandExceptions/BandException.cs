namespace RftmAPI.Domain.Exceptions.BandExceptions;

/// <summary>
/// Исключение доменной модели музыкальной группы.
/// </summary>
public abstract class BandException : DomainException
{
    /// <summary>
    /// Создание исключения доменной модели музыкальной группы.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    protected BandException(string message) : base($"{nameof(BandException)}.{message}")
    {
    }
}