namespace RftmAPI.Domain.Primitives;

/// <summary>
/// Представление резульата доменной модели.
/// </summary>
/// <typeparam name="TValue">Тип результата</typeparam>
public class Result<TValue> : FluentResults.Result<TValue>
{
    /// <summary>
    /// Перегрузка неявного приведения типов.
    /// </summary>
    /// <param name="exception">Исключение.</param>
    /// <returns>Результат с исключением.</returns>
    public static implicit operator Result<TValue>(Exception exception) => (Result<TValue>) FluentResults.Result.Fail<TValue>(exception.Message);

    /// <summary>
    /// Перегрузка неявного приведения типов.
    /// </summary>
    /// <param name="value">Значение результата.</param>
    /// <returns>Результат.</returns>
    public static implicit operator Result<TValue>(TValue value) => (Result<TValue>) FluentResults.Result.Ok(value);
}