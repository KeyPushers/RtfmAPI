using RftmAPI.Domain.Errors.TrackErrors;
using RftmAPI.Domain.Primitives;
using RftmAPI.Domain.Shared;

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
            return Result<TrackFileName>.Failure(TrackErrors.TrackFileName.IsNullOrWhiteSpace);
        }

        if (value.Length < MinLength)
        {
            return Result<TrackFileName>.Failure(TrackErrors.TrackFileName.IsTooShort);
        }

        if (value.Length > MaxLength)
        {
            return Result<TrackFileName>.Failure(TrackErrors.TrackFileName.IsTooLong);
        }

        if (value.Trim().Any(sign => Path.GetInvalidFileNameChars().Contains(sign)))
        {
            return Result<TrackFileName>.Failure(TrackErrors.TrackFileName.Invalid);
        }
        
        return new TrackFileName(value);
    }

    /// <inheritdoc cref="ValueObject.GetEqualityComponents"/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}