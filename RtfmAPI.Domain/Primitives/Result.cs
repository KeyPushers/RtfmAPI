namespace RftmAPI.Domain.Primitives;

/// <summary>
/// Представление результата операции.
/// </summary>
/// <typeparam name="TValue">Тип результирующего значения.</typeparam>
public sealed class Result<TValue> : BaseResult
{
    private readonly TValue _value;

    /// <summary>
    /// Создание представление результата операции.
    /// </summary>
    /// <param name="value">Значение результата операции.</param>
    /// <param name="error">Ошибка операции.</param>
    private Result(TValue value, Error error) : base(error)
    {
        _value = value;
    }

    /// <summary>
    /// Создание представление результата операции.
    /// </summary>
    /// <param name="value">Значение результата операции.</param>
    public static Result<TValue> Create(TValue value)
    {
        return new(value, Error.None);
    }

    /// <summary>
    /// Создание представление результата операции.
    /// </summary>
    /// <param name="error">Ошибка операции.</param>
    public static Result<TValue> Create(Error error)
    {
        return new(default!, error);
    }
    
    /// <summary>
    /// Получение значения результата операции.
    /// </summary>
    /// <exception cref="InvalidOperationException">Невозможно получить значение результата операции с ошибкой.</exception>
    public TValue Value => IsSuccess
        ? _value
        : throw new InvalidOperationException("Невозможно получить значение результата операции с ошибкой.");

    /// <summary>
    /// Перегрузка оператора неявного приведения типа к <see cref="Result{TValue}"/>.
    /// </summary>
    /// <param name="value">Значение.</param>
    /// <returns>Представления успешного результата.</returns>
    public static implicit operator Result<TValue>(TValue value) => Create(value);

    /// <summary>
    /// Перегрузка оператора неявного приведения типа к <see cref="Result{TValue}"/>.
    /// </summary>
    /// <param name="exception">Исключение.</param>
    /// <returns>Представления результата с ошибкой.</returns>
    public static implicit operator Result<TValue>(Exception exception) => Create(exception);
}