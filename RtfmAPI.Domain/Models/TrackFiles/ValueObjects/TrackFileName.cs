using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.TrackFiles.ValueObjects;

/// <summary>
/// Название файла музыкального трека.
/// </summary>
public sealed class TrackFileName : ValueObject
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
            return new InvalidOperationException();
        }

        if (value.Length < MinLength)
        {
            return new InvalidOperationException();
        }

        if (value.Length > MaxLength)
        {
            return new InvalidOperationException();
        }

        if (value.Trim().Any(sign => Path.GetInvalidFileNameChars().Contains(sign)))
        {
            return new InvalidOperationException();
        }

        return new TrackFileName(value);
    }

    /// <inheritdoc cref="ValueObject.GetEqualityComponents"/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}