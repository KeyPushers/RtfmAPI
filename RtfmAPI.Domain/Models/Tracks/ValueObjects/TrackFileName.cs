using RftmAPI.Domain.Exceptions.TrackExceptions;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Tracks.ValueObjects;

/// <summary>
/// Название файла музыкального трека.
/// </summary>
public class TrackFileName : ValueObject
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
    /// Создание названия файла музыкального трека.
    /// </summary>
    /// <param name="value">Значение.</param>
    private TrackFileName(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Значение названия.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Создание названия файла музыкального трека.
    /// </summary>
    /// <param name="value">Значение.</param>
    /// <returns>Название файла музыкального трека.</returns>
    public static Result<TrackFileName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return TrackExceptions.TrackFileNameExceptions.IsNullOrWhiteSpace;
        }

        if (value.Length < MinLength)
        {
            return TrackExceptions.TrackFileNameExceptions.IsTooShort;
        }

        if (value.Length > MaxLength)
        {
            return TrackExceptions.TrackFileNameExceptions.IsTooLong;
        }

        if (value.Trim().Any(sign => Path.GetInvalidFileNameChars().Contains(sign)))
        {
            return TrackExceptions.TrackFileNameExceptions.Invalid;
        }

        return new TrackFileName(value);
    }

    /// <inheritdoc cref="ValueObject.GetEqualityComponents"/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}