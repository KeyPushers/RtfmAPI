namespace RftmAPI.Domain.Primitives;

/// <summary>
/// Представление результата доменной операции.
/// </summary>
public class BaseResult
{
    /// <summary>
    /// Инициализация представления результата операции.
    /// </summary>
    /// <param name="error">Ошибка операции.</param>
    protected BaseResult(Error error)
    {
        IsSuccess = error == Error.None;
        Error = error;
    }

    /// <summary>
    /// Признак успешного выполнения операции.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Признак ошибки при выполнении операции.
    /// </summary>
    public bool IsFailed => !IsSuccess;

    /// <summary>
    /// Ошибка операции.
    /// </summary>
    public Error Error { get; }

    /// <summary>
    /// Создание представления результата успешной операции.
    /// </summary>
    /// <returns>Представление результата операции.</returns>
    protected static BaseResult Success() => new(Error.None);

    /// <summary>
    /// Создание представления результата операции, закончившейся ошибкой.
    /// </summary>
    /// <returns>Представление результата операции.</returns>
    protected static BaseResult Failure(Error error) => new(error);
}