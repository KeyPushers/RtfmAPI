

using FluentResults;

namespace RftmAPI.Domain.Primitives;

/// <summary>
/// Представление доменной ошибки.
/// </summary>
public class Error : ExceptionalError
{
    /// <summary>
    /// Создание представления доменной ошибки.
    /// </summary>
    /// <param name="exception">Исключение.</param>
    public Error(Exception exception) : base(exception)
    {
    }

    /// <summary>
    /// Создание представления доменной ошибки.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    /// <param name="exception">Исключение.</param>
    public Error(string message, Exception exception) : base(message, exception)
    {
    }
    
    /// <summary>
    /// Перегрузка оператора неявного приведения типа.
    /// </summary>
    /// <param name="exception">Исключение.</param>
    /// <returns>Представление доменной ошибки.</returns>
    public static implicit operator Error(Exception exception) => new(exception);
}