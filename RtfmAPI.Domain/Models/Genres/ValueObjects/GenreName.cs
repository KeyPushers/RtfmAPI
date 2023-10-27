using RftmAPI.Domain.Exceptions.GenreExceptions;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Genres.ValueObjects;

/// <summary>
/// Название музыкального жанра.
/// </summary>
public class GenreName : ValueObject
{
    /// <summary>
    /// Минимальная длина названия.
    /// </summary>
    public const int MinLength = 1;

    /// <summary>
    /// Максимальная длина названия.
    /// </summary>
    public const int MaxLength = 50;

    /// <summary>
    /// Название музыкального жанра.
    /// </summary>
    /// <param name="value">Значение.</param>
    private GenreName(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Название.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Создание названия музыкального жанра.
    /// </summary>
    /// <param name="value">Значение.</param>
    /// <returns>Название музыкального жанра.</returns>
    public static Result<GenreName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return GenreExceptions.GenreNameExceptions.IsNullOrWhiteSpace;
        }

        if (value.Length < MinLength)
        {
            return GenreExceptions.GenreNameExceptions.IsTooShort;
        }

        if (value.Length > MaxLength)
        {
            return GenreExceptions.GenreNameExceptions.IsTooLong;
        }

        return new GenreName(value);
    }

    /// <summary>
    /// <inheritdoc cref="ValueObject.GetEqualityComponents"/>
    /// </summary>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}