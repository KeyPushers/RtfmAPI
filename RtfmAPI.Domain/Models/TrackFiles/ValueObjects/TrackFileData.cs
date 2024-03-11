using System.Collections.Generic;
using System.Linq;
using RtfmAPI.Domain.Models.TrackFiles.Exceptions;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.TrackFiles.ValueObjects;

/// <summary>
/// Содержимое файла музыкального трека.
/// </summary>
public sealed class TrackFileData : ValueObject
{
    /// <summary>
    /// Создание содержимого файла музыкального трека.
    /// </summary>
    /// <param name="value">Значение.</param>
    private TrackFileData(byte[] value)
    {
        Value = value;
    }

    /// <summary>
    /// Содержимое.
    /// </summary>
    public byte[] Value { get; }

    /// <summary>
    /// Создание содержимого файла музыкального трека.
    /// </summary>
    /// <param name="value">Значение.</param>
    /// <returns>Содержимое файла музыкального трека.</returns>
    public static Result<TrackFileData> Create(byte[] value)
    {
        if (!value.Any())
        {
            return TrackFileExceptions.TrackFileDataIsEmpty();
        }

        return new TrackFileData(value);
    }

    /// <inheritdoc cref="ValueObject.GetEqualityComponents"/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}