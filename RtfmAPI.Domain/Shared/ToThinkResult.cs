namespace RftmAPI.Domain.Shared;

/// <summary>
/// Представление результата доменной операции.
/// </summary>
public class ToThinkResult
{
    /// <summary>
    /// Инициализация представления результата операции.
    /// </summary>
    /// <param name="isSuccess">Признак успешного выполнения операции.</param>
    /// <param name="error">Ошибка операции.</param>
    protected internal ToThinkResult(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error != Error.None)
        {
            throw new InvalidOperationException();
        }

        
        IsSuccess = isSuccess;
        Error = error;
    }
    
    /// <summary>
    /// Признак успешного выполнения операции.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Признак ошибки при выполнении операции.
    /// </summary>
    public bool IsFailure => !IsSuccess;
    
    /// <summary>
    /// Ошибка операции.
    /// </summary>
    public Error Error { get; }
    
    /// <summary>
    /// Создание представления результата успешной операции.
    /// </summary>
    /// <returns>Представление результата операции.</returns>
    public static ToThinkResult Success() => new(true, Error.None);

    /// <summary>
    /// Создание представления результата успешной операции.
    /// </summary>
    /// <param name="value">Знаечния результата операции.</param>
    /// <typeparam name="TValue">Тип значения результата операции.</typeparam>
    /// <returns>Представление результата операции.</returns>
    public static ToThinkOperationResult<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    /// <summary>
    /// Создание представления результата операции, закончившейся ошибкой.
    /// </summary>
    /// <returns>Представление результата операции.</returns>
    public static ToThinkResult Failure(Error error) => new(false, error);

    /// <summary>
    /// Создание представления результата операции, закончившейся ошибкой.
    /// </summary>
    /// <param name="error">Представление ошибки доменной операции.</param>
    /// <typeparam name="TValue">Тип значения результата операции.</typeparam>
    /// <returns>Представление результата операции.</returns>
    public static ToThinkOperationResult<TValue> Failure<TValue>(Error error) => new(default!, false, error);

    /// <summary>
    /// Создание представления результата доменной операции с определенным значением и ошибкой.
    /// </summary>
    /// <param name="value">Значение результата операции.</param>
    /// <param name="error">Ошибка операции.</param>
    /// <typeparam name="TValue">Тип значения результата операции.</typeparam>
    /// <returns>Представление результата доменной операции.</returns>
    public static ToThinkOperationResult<TValue> Create<TValue>(TValue? value, Error error) where TValue : class
    {
        return value is null ? Failure<TValue>(error) : Success(value);
    }
}