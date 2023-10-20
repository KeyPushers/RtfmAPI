using RftmAPI.Domain.Errors.TrackErrors;
using RftmAPI.Domain.Primitives;
using RftmAPI.Domain.Shared;

namespace RftmAPI.Domain.Models.Tracks.ValueObjects;

/// <summary>
/// Расширение файла музыкального трека.
/// </summary>
public class TrackFileExtension : ValueObject
{
    /// <summary>
    /// Создание расширения файла музыкального трека.
    /// </summary>
    /// <param name="value">Значение.</param>
    private TrackFileExtension(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Название.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Создание представления данных музыкального трека.
    /// </summary>
    /// <param name="value">Значение.</param>
    /// <returns>Расширение файла музыкального трека.</returns>
    public static Result<TrackFileExtension> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<TrackFileExtension>.Failure(TrackErrors.TrackFileExtension.IsNullOrWhiteSpace);
        }

        var trimmedValue = value.Trim();
        
        if (trimmedValue.Any(sign => Path.GetInvalidFileNameChars().Contains(sign)))
        {
            return Result<TrackFileExtension>.Failure(TrackErrors.TrackFileExtension.Invalid);
        }

        if (trimmedValue.StartsWith(".") || trimmedValue.EndsWith("."))
        {
            return Result<TrackFileExtension>.Failure(TrackErrors.TrackFileExtension.Invalid);
        }

        return new TrackFileExtension(value);
    }

    /// <inheritdoc cref="ValueObject.GetEqualityComponents"/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}