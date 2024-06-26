﻿using System.Collections.Generic;
using RtfmAPI.Domain.Models.TrackFiles.Exceptions;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.TrackFiles.ValueObjects;

/// <summary>
/// MIME-тип файла музыкального трека.
/// </summary>
public sealed class TrackFileMimeType : ValueObject
{
    /// <summary>
    /// MIME-типы аудио.
    /// </summary>
    private static readonly HashSet<string> AudioMediaTypes = new()
    {
        "audio/basic",
        "audio/L24",
        "audio/mp4",
        "audio/aac",
        "audio/mpeg",
        "audio/ogg",
        "audio/vorbis",
        "audio/x-ms-wma",
        "audio/x-ms-wax",
        "audio/vnd.rn-realaudio",
        "audio/vnd.wave",
        "audio/webm"
    };

    /// <summary>
    /// Создание MIME-тип файла музыкального трека.
    /// </summary>
    /// <param name="value">Значение.</param>
    private TrackFileMimeType(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Значение.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Создание  MIME-типа файла музыкального трека.
    /// </summary>
    /// <param name="value">Значение.</param>
    /// <returns>Название файла музыкального трека.</returns>
    public static Result<TrackFileMimeType> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return TrackFileExceptions.TrackFileMimeTypeIsNullOrEmpty();
        }

        if (!AudioMediaTypes.Contains(value))
        {
            return TrackFileExceptions.TrackFileMimeTypeUnknown();
        }

        return new TrackFileMimeType(value);
    }

    /// <inheritdoc cref="ValueObject.GetEqualityComponents"/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}