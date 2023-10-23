namespace RftmAPI.Domain.Primitives;

/// <summary>
/// Представление ошибки доменной модели.
/// </summary>
public class Error : FluentResults.ExceptionalError
{
    /// <summary>
    /// Создание представления ошибки доменной модели.
    /// </summary>
    /// <param name="exception">Исключение.</param>
    public Error(Exception exception) : base(exception)
    {
    }
}