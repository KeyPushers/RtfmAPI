using RftmAPI.Domain.Exceptions.TrackExceptions;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Tracks.ValueObjects;

/// <summary>
/// Продолжительность музыкального трека.
/// </summary>
public sealed class TrackDuration : ValueObject
{
    /// <summary>
    /// Значение в миллисекундах.
    /// </summary>
    public double Value { get; }

    /// <summary>
    /// Создание продолжительности музыкального трека.
    /// </summary>
    /// <param name="value">Продолжительность музыкального трека в миллисекундах.</param>
    private TrackDuration(double value)
    {
        Value = value;
    }

    /// <summary>
    /// Создание продолжительности музыкального трека.
    /// </summary>
    /// <param name="value">Значение.</param>
    /// <returns>Продолжительность музыкального трека.</returns>
    public static Result<TrackDuration> Create(double value)
    {
        if (value < 0)
        {
            return TrackExceptions.TrackDurationExceptions.IncorrectDuration;
        }

        return new TrackDuration(value);
    }

    /// <summary>
    /// <inheritdoc cref="ValueObject.GetEqualityComponents"/>
    /// </summary>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}