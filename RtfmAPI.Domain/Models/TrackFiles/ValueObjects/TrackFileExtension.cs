using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentResults;
using RtfmAPI.Domain.Models.TrackFiles.Errors;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.TrackFiles.ValueObjects;

/// <summary>
/// Расширение файла музыкального трека.
/// </summary>
public sealed class TrackFileExtension : ValueObject
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
            return TrackFileErrors.TrackFileExtensionIsNullOrEmpty();
        }

        var trimmedValue = value.Trim();

        if (trimmedValue.Any(sign => Path.GetInvalidFileNameChars().Contains(sign)))
        {
            return TrackFileErrors.TrackFileExtensionIsInvalid();
        }

        if (trimmedValue.EndsWith("."))
        {
            return TrackFileErrors.TrackFileExtensionIsInvalid();
        }

        return new TrackFileExtension(value);
    }

    /// <inheritdoc cref="ValueObject.GetEqualityComponents"/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}