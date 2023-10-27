namespace RftmAPI.Domain.Primitives;

/// <summary>
/// Представления результата.
/// </summary>
/// <typeparam name="TValue">Значение результата.</typeparam>
public class Result<TValue> : FluentResults.Result<TValue>
{
    /// <summary>
    /// Значение результата.
    /// </summary>
    public new TValue Value => base.Value;

    /// <summary>
    /// Сообщение об ошибке.
    /// </summary>
    public string Error => string.Join(";\n", Errors);
    
    /// <summary>
    /// Перегрузка оператора неявного приведения типа к <see cref="Result{TValue}"/>.
    /// </summary>
    /// <param name="value">Значение.</param>
    /// <returns>Представления успешного результата.</returns>
    public static implicit operator Result<TValue>(TValue value) => Success(value);

    /// <summary>
    /// Перегрузка оператора неявного приведения типа к <see cref="Result{TValue}"/>.
    /// </summary>
    /// <param name="exception">Исключение.</param>
    /// <returns>Представления результата с ошибкой.</returns>
    public static implicit operator Result<TValue>(Exception exception) => Failure(exception);

    /// <summary>
    /// Создание представления успешного результата.
    /// </summary>
    /// <param name="value">Значение.</param>
    /// <returns>Представления успешного результата.</returns>
    private static Result<TValue> Success(TValue value)
    {
        return Create(new Result<TValue>().WithValue(value));
    }
    
    /// <summary>
    /// Создание представления результата с ошибкой.
    /// </summary>
    /// <param name="exception">Исключение.</param>
    /// <returns>Представления результата с ошибкой.</returns>
    private static Result<TValue> Failure(Exception exception)
    {
        Error error = exception;
        
        return Create(new Result<TValue>().WithError(error));
    }
    
    /// <summary>
    /// Создание представления результата.
    /// </summary>
    /// <param name="result">Результата типа <see cref="FluentResults.Result{TValue}"/>.</param>
    /// <returns>Представления результата.</returns>
    private static Result<TValue> Create(FluentResults.Result<TValue> result)
    {
        return (Result<TValue>) result;
    }
}