using System;
using System.Collections.Generic;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Albums.ValueObjects;

/// <summary>
/// Дата выпуска музыкального альбома.
/// </summary>
public sealed class AlbumReleaseDate : ValueObject
{
    /// <summary>
    /// Дата выпуска музыкального альбома.
    /// </summary>
    /// <param name="value">Значение.</param>
    private AlbumReleaseDate(DateTime value)
    {
        Value = value;
    }

    /// <summary>
    /// Значение.
    /// </summary>
    public DateTime Value { get; }

    /// <summary>
    /// Создание даты выпуска музыкального альбома.
    /// </summary>
    /// <param name="value">Дата.</param>
    /// <returns>Дата выпуска музыкального альбома.</returns>
    public static Result<AlbumReleaseDate> Create(DateTime value)
    {
        if (value == DateTime.MinValue)
        {
            return new ArgumentOutOfRangeException();
        }

        var date = value.Kind is DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(value, DateTimeKind.Utc)
            : value.ToUniversalTime();
        return new AlbumReleaseDate(date);
    }

    /// <inheritdoc cref="ValueObject.GetEqualityComponents"/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}