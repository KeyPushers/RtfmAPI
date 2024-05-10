using System.Collections.Generic;
using FluentResults;
using RtfmAPI.Domain.Models.Tracks.Errors;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Tracks.ValueObjects;

/// <summary>
/// Название музыкального трека.
/// </summary>
public sealed class TrackName : ValueObject
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
    /// Название музыкального трека.
    /// </summary>
    /// <param name="value">Значение.</param>
    private TrackName(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Название.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Создание названия музыкального трека.
    /// </summary>
    /// <param name="value">Значение.</param>
    /// <returns>Название музыкального трека.</returns>
    public static Result<TrackName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return TrackErrors.TrackNameIsNullOrEmpty();
        }

        if (value.Length < MinLength)
        {
            return TrackErrors.TrackNameIsTooShort();
        }

        if (value.Length > MaxLength)
        {
            return TrackErrors.TrackNameIsTooLong();
        }

        return new TrackName(value);
    }

    /// <summary>
    /// <inheritdoc cref="ValueObject.GetEqualityComponents"/>
    /// </summary>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}