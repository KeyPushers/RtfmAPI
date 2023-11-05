using RftmAPI.Domain.Exceptions.TrackFileExceptions;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.TrackFiles.ValueObjects;

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
            return TrackFileExceptions.TrackFileExtensionExceptions.IsNullOrWhiteSpace;
        }

        var trimmedValue = value.Trim();

        if (trimmedValue.Any(sign => Path.GetInvalidFileNameChars().Contains(sign)))
        {
            return TrackFileExceptions.TrackFileExtensionExceptions.Invalid;
        }

        if (trimmedValue.EndsWith("."))
        {
            return TrackFileExceptions.TrackFileExtensionExceptions.Invalid;
        }

        return new TrackFileExtension(value);
    }

    /// <inheritdoc cref="ValueObject.GetEqualityComponents"/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}