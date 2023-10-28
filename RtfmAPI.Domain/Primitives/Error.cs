namespace RftmAPI.Domain.Primitives;

/// <summary>
/// Представление ошибки доменной операции.
/// </summary>
public sealed class Error : IEquatable<Error>
{
    /// <summary>
    /// Отсутствие ошибки.
    /// </summary>
    public static readonly Error None = new(string.Empty);
 
    /// <summary>
    /// Представление ошибки в видео исключения.
    /// </summary>
    private readonly Exception _exception;
    
    /// <summary>
    /// Создание представления ошибки доменной операции.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    public Error(string message)
    {
        _exception = new Exception(message);
    }

    /// <summary>
    /// Создание представления ошибки доменной операции.
    /// </summary>
    /// <param name="exception">Исключение.</param>
    public Error(Exception exception)
    {
        _exception = exception;
    }
    
    /// <summary>
    /// Сообщение об ошибке.
    /// </summary>
    public string Message => _exception.Message;
    
    /// <summary>
    /// Перегрузка неявного приведения <see cref="Error"/> к типу <see cref="string"/>.
    /// </summary>
    /// <param name="error">Представление ошибки доменной операции.</param>
    /// <returns>Объект типа <see cref="string"/></returns>
    public static implicit operator string(Error error) => error.Message;

    /// <summary>
    /// Перегрузка неявного приведения <see cref="string"/> к типу <see cref="Error"/>.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <returns>Представление ошибки доменной операции.</returns>
    public static implicit operator Error(string message) => new(message);
    
    /// <summary>
    /// Перегрузка неявного приведения <see cref="Exception"/> к типу <see cref="Error"/>.
    /// </summary>
    /// <param name="exception">Исключение.</param>
    /// <returns>Представление ошибки доменной операции.</returns>
    public static implicit operator Error(Exception exception) => new(exception);
    
    /// <summary>
    /// Получение сравниваемых компонентов объекта.
    /// </summary>
    /// <returns>Сравниваемые компоненты объекта.</returns>
    private IEnumerable<object> GetEqualityComponents()
    {
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