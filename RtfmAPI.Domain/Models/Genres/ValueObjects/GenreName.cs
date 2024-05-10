using System.Collections.Generic;
using FluentResults;
using RtfmAPI.Domain.Models.Genres.Errors;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Genres.ValueObjects;

/// <summary>
/// Название музыкального жанра.
/// </summary>
public sealed class GenreName : ValueObject
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
            return GenreErrors.GenreNameIsNullOrEmpty();
        }

        if (value.Length < MinLength)
        {
            return GenreErrors.GenreNameIsTooShort();
        }

        if (value.Length > MaxLength)
        {
            return GenreErrors.GenreNameIsTooLong();
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