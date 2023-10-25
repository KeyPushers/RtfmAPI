﻿using FluentResults;
using RftmAPI.Domain.Exceptions.TrackExceptions;
using RftmAPI.Domain.Primitives;

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
            return new ExceptionalError(TrackExceptions.TrackFileExtensionExceptions.IsNullOrWhiteSpace);
        }

        var trimmedValue = value.Trim();
        
        if (trimmedValue.Any(sign => Path.GetInvalidFileNameChars().Contains(sign)))
        {
            return new ExceptionalError(TrackExceptions.TrackFileExtensionExceptions.Invalid);
        }

        if (trimmedValue.EndsWith("."))
        {
            return new ExceptionalError(TrackExceptions.TrackFileExtensionExceptions.Invalid);
        }

        return new TrackFileExtension(value);
    }

    /// <inheritdoc cref="ValueObject.GetEqualityComponents"/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}