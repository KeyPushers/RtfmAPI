namespace RftmAPI.Domain.Shared;

/// <summary>
/// Представление результата операции.
/// </summary>
/// <typeparam name="TValue">Тип результирующего значения.</typeparam>
public class ToThinkOperationResult<TValue> : ToThinkResult
{
    private readonly TValue _value;

    /// <summary>
    /// Инициализация представление результата операции.
    /// </summary>
    /// <param name="value">Значение результата операции.</param>
    /// <param name="isSuccess">Признак успешного выполнения операции.</param>
    /// <param name="error">Ошибка операции.</param>
    protected internal ToThinkOperationResult(TValue value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        _value = value;
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
    /// к типу представления результата операции <see cref="ToThinkOperationResult{TValue}"/>.
    /// </summary>
    /// <param name="value">Преобразовываемое значение.</param>
    /// <returns>Представление результата операции.</returns>
    public static implicit operator ToThinkOperationResult<TValue>(TValue value) => Success(value);
}