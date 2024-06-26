﻿using System.Collections.Generic;
using RtfmAPI.Domain.Models.Bands.Exceptions;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Bands.ValueObjects;

/// <summary>
/// Название музыкальной группы.
/// </summary>
public sealed class BandName : ValueObject
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
    /// Название музыкальной группы.
    /// </summary>
    /// <param name="value">Значение.</param>
    private BandName(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Название.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Создание названия музыкальной группы.
    /// </summary>
    /// <param name="value">Значение.</param>
    /// <returns>Название музыкальной группы.</returns>
    public static Result<BandName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return BandExceptions.BandNameIsNullOrEmpty();
        }

        if (value.Length < MinLength)
        {
            return BandExceptions.BandNameIsTooShort();
        }

        if (value.Length > MaxLength)
        {
            return BandExceptions.BandNameIsTooLong();
        }

        return new BandName(value);
    }

    /// <inheritdoc cref="ValueObject.GetEqualityComponents"/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}