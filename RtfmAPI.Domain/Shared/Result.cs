namespace RftmAPI.Domain.Shared;

/// <summary>
/// Представление результата операции.
/// </summary>
/// <typeparam name="TValue">Тип результирующего значения.</typeparam>
/// <remarks>Объяснение: <see cref="https://youtu.be/KgfzM0QWHrQ?si=eWDXVr8_gIhYx7zq"/></remarks>
public class Result<TValue> where TValue : class
{
    private readonly TValue _value;

    /// <summary>
    /// Инициализация представление результата операции.
    /// </summary>
    /// <param name="value">Значение результата операции.</param>
    /// <param name="isSuccess">Признак успешного выполнения операции.</param>
    /// <param name="error">Ошибка операции.</param>
    private Result(TValue value, bool isSuccess, Error error)
    {
        _value = value;

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
    /// Получение значения результата операции.
    /// </summary>
    /// <exception cref="InvalidOperationException">Невозможно получить значение результата операции с ошибкой.</exception>
    public TValue Value => IsSuccess
        ? _value
        : throw new InvalidOperationException("Невозможно получить значение результата операции с ошибкой.");

    /// <summary>
    /// Перегрузка оператора неявного преобразования значения результата операции <see cref="TValue"/>
    /// к типу представления результата операции <see cref="Result{TValue}"/>.
    /// </summary>
    /// <param name="value">Преобразовываемое значение.</param>
    /// <returns>Представление результата операции.</returns>
    public static implicit operator Result<TValue>(TValue value) => Success(value);

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
    /// <param name="value">Знаечния результата операции.</param>
    /// <typeparam name="TValue">Тип значения результата операции.</typeparam>
    /// <returns>Представление результата операции.</returns>
    public static Result<TValue> Success(TValue value) => new(value, true, Error.None);

    /// <summary>
    /// Создание представления результата операции, закончившейся ошибкой.
    /// </summary>
    /// <param name="error">Представление ошибки доменной операции.</param>
    /// <typeparam name="TValue">Тип значения результата операции.</typeparam>
    /// <returns>Представление результата операции.</returns>
    public static Result<TValue> Failure(Error error) => new(default!, false, error);

    /// <summary>
    /// Создание представления результата доменной операции с определенным значением и ошибкой.
    /// </summary>
    /// <param name="value">Значение результата операции.</param>
    /// <param name="error">Ошибка операции.</param>
    /// <typeparam name="TValue">Тип значения результата операции.</typeparam>
    /// <returns>Представление результата доменной операции.</returns>
    public static Result<TValue> Create(TValue? value, Error error)
    {
        return value is null ? Failure(error) : Success(value);
    }
}