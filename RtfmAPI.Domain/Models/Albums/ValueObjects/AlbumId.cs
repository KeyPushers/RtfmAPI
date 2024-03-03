using System;
using System.Collections.Generic;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Albums.ValueObjects;

/// <summary>
/// Идентификатор альбома.
/// </summary>
public sealed class AlbumId : AggregateRootId<Guid>
{
    /// <summary>
    /// Значение идентификатора.
    /// </summary>
    public override Guid Value { get; }

    /// <summary>
    /// Идентификатор альбома.
    /// </summary>
    /// <param name="value">Значение идентификатора.</param>
    private AlbumId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Создание идентификатора музыкального альбома.
    /// </summary>
    /// <returns>Идентификатор музыкального альбома.</returns>
    public static AlbumId Create()
    {
        return new AlbumId(Guid.NewGuid());
    }

    /// <summary>
    /// Создание идентификатора альбома.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <returns>Идентификатор музыкального альбома.</returns>
    public static AlbumId Create(Guid id)
    {
        return new AlbumId(id);
    }

    /// <inheritdoc cref="ValueObject.GetEqualityComponents"/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}