using System;
using System.Collections.Generic;
using RtfmAPI.Domain.Models.Genres.Exceptions;
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
            return GenreExceptions.GenreNameIsNullOrEmpty();
        }

        if (value.Length < MinLength)
        {
            return GenreExceptions.GenreNameIsTooShort();
        }

        if (value.Length > MaxLength)
        {
            return GenreExceptions.GenreNameIsTooLong();
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