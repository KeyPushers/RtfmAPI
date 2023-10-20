using RftmAPI.Domain.Errors.TrackErrors;
using RftmAPI.Domain.Primitives;
using RftmAPI.Domain.Shared;

namespace RftmAPI.Domain.Models.Tracks.ValueObjects;

/// <summary>
/// Название музыкального трека.
/// </summary>
public class TrackName : ValueObject
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
            return Result<TrackName>.Failure(TrackErrors.TrackName.IsNullOrWhiteSpace);
        }

        if (value.Length < MinLength)
        {
            return Result<TrackName>.Failure(TrackErrors.TrackName.IsTooShort);
        }

        if (value.Length > MaxLength)
        {
            return Result<TrackName>.Failure(TrackErrors.TrackName.IsTooLong);
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