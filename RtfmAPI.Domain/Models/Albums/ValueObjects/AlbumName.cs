using System.Collections.Generic;
using FluentResults;
using RtfmAPI.Domain.Models.Albums.Errors;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Albums.ValueObjects;

/// <summary>
/// Название музыкального альбома.
/// </summary>
public sealed class AlbumName : ValueObject
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
    /// Название альбома.
    /// </summary>
    /// <param name="value">Значение.</param>
    private AlbumName(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Название музыкального альбома.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Создание названия музыкального альбома.
    /// </summary>
    /// <param name="value">Значение.</param>
    /// <returns>Название музыкального альбома.</returns>
    public static Result<AlbumName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return AlbumErrors.AlbumNameIsNullOrEmpty();
        }

        if (value.Length < MinLength)
        {
            return AlbumErrors.AlbumNameIsTooShort();
        }

        if (value.Length > MaxLength)
        {
            return AlbumErrors.AlbumNameIsTooLong();
        }

        return new AlbumName(value);
    }

    /// <inheritdoc cref="ValueObject.GetEqualityComponents"/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}