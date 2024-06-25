using System;
using System.Collections.Generic;
using RtfmAPI.Domain.Models.Tracks.Exceptions;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Tracks.ValueObjects;

/// <summary>
/// Дата выпуска музыкального трека.
/// </summary>
public sealed class TrackReleaseDate : ValueObject
{
    /// <summary>
    /// Дата выпуска музыкального альбома.
    /// </summary>
    /// <param name="value">Значение.</param>
    private TrackReleaseDate(DateTime value)
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
    public static Result<TrackReleaseDate> Create(DateTime value)
    {
        if (value == DateTime.MinValue)
        {
            return TrackExceptions.InvalidTrackReleaseDate();
        }

        var date = value.Kind is DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(value, DateTimeKind.Utc)
            : value.ToUniversalTime();
        return new TrackReleaseDate(date);
    }

    /// <inheritdoc cref="ValueObject.GetEqualityComponents"/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}