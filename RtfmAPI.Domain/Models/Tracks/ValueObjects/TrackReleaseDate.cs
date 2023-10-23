using RftmAPI.Domain.Exceptions.TrackExceptions;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Tracks.ValueObjects;

/// <summary>
/// Дата выпуска музыкального трека.
/// </summary>
public class TrackReleaseDate : ValueObject
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
            return TrackExceptions.TrackReleaseDateExceptions.InvalidDate;
        }

        return new TrackReleaseDate(value);
    }

    /// <inheritdoc cref="ValueObject.GetEqualityComponents"/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}