namespace RftmAPI.Domain.Exceptions;

/// <summary>
/// Исключение доменных моделей.
/// </summary>
public abstract class DomainException : Exception
{
    /// <summary>
    /// Создание исключения доменной модели.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    protected DomainException(string message) : base($"{nameof(DomainException)}.{message}")
    {
    }
}