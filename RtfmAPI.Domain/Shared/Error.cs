using RftmAPI.Domain.Errors;

namespace RftmAPI.Domain.Shared;

/// <summary>
/// Представление ошибки доменной операции.
/// </summary>
public sealed class Error : IEquatable<Error>
{
    /// <see cref="SharedErrors.None"/>
    public static readonly Error None = SharedErrors.None;

    /// <see cref="SharedErrors.NullValue"/>
    public static readonly Error NullValue = SharedErrors.NullValue;
    
    /// <summary>
    /// Инициализация представления ошибки доменной операции.
    /// </summary>
    /// <param name="code">Код ошибки.</param>
    /// <param name="message">Сообщение об ошибке.</param>
    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }
    
    /// <summary>
    /// Код ошибки.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Сообщение об ошибке.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Перегрузка неявного приведения <see cref="Error"/> к типу <see cref="string"/>
    /// </summary>
    /// <param name="error">Представление ошибки доменной операции.</param>
    /// <returns>Объект типа <see cref="string"/></returns>
    public static implicit operator string(Error? error) => error?.Code ?? string.Empty;
    
    /// <summary>
    /// Получение сравниваемых компонентов объекта.
    /// </summary>
    /// <returns>Сравниваемые компоненты объекта.</returns>
    private IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
        yield return Message;
    }
    
    /// <summary>
    /// Получение признака равенства объектов.
    /// </summary>
    /// <param name="other">Сравниваемый объект.</param>
    /// <returns>Признак равенства объектов</returns>
    public bool Equals(Error? other)
    {
        return Equals((object?)other);
    }

    /// <summary>
    /// Получение признака равенства объектов.
    /// </summary>
    /// <param name="obj">Сравниваемый объект.</param>
    /// <returns>Признак равенства объектов</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not Error error)
        {
            return false;
        }

        return GetEqualityComponents()
            .SequenceEqual(error.GetEqualityComponents());
    }

    /// <summary>
    /// Получение хэш-кода.
    /// </summary>
    /// <returns>Хэш-код.</returns>
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x.GetHashCode())
            .Aggregate((x, y) => x ^ y);
    }
    
    /// <summary>
    /// Перегрузка оператора "==".
    /// </summary>
    /// <param name="left">Значение слева от оператора.</param>
    /// <param name="right">Значение справа от оператора.</param>
    /// <returns>Признак раветства объектов.</returns>
    public static bool operator ==(Error? left, Error? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Перегрузка оператора "!=".
    /// </summary>
    /// <param name="left">Значение слева от оператора.</param>
    /// <param name="right">Значение справа от оператора.</param>
    /// <returns>Признак нераветства объектов.</returns>
    public static bool operator !=(Error? left, Error? right)
    {
        return !Equals(left, right);
    }
}