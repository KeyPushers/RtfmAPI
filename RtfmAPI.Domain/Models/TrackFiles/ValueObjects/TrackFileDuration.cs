using System;
using System.Collections.Generic;
using RtfmAPI.Domain.Models.TrackFiles.Exceptions;
using RtfmAPI.Domain.Models.Tracks.Exceptions;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.TrackFiles.ValueObjects;

/// <summary>
/// Продолжительность файла музыкального трека.
/// </summary>
public class TrackFileDuration : ValueObject
{
    /// <summary>
    /// Значение в миллисекундах.
    /// </summary>
    public double Value { get; private set; }

    /// <summary>
    /// Создание продолжительности файла музыкального трека.
    /// </summary>
    /// <param name="value">Продолжительность файла музыкального трека в миллисекундах.</param>
    private TrackFileDuration(double value)
    {
        Value = value;
    }

    /// <summary>
    /// Создание продолжительности файла музыкального трека.
    /// </summary>
    /// <param name="value">Значение.</param>
    /// <returns>Продолжительность музыкального трека.</returns>
    public static Result<TrackFileDuration> Create(double value)
    {
        if (value < 0)
        {
            return TrackFileExceptions.TrackFileDurationIsInvalid();
        }

        return new TrackFileDuration(value);
    }

    /// <summary>
    /// <inheritdoc cref="ValueObject.GetEqualityComponents"/>
    /// </summary>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}