using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Aggregates.Tracks;

/// <summary>
/// Идентификатор альбома
/// </summary>
public class AlbumId : ValueObject
{
    public Guid Value { get; private init; }

    /// <summary>
    /// Идентификатор альбома
    /// </summary>
    /// <param name="id">Идентификатор</param>
    public AlbumId(Guid id)
    {
        Value = id;
    }
}