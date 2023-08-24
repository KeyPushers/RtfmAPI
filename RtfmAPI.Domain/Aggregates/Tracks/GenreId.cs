using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Aggregates.Tracks;

/// <summary>
/// Идентификатор жанра
/// </summary>
public class GenreId : ValueObject
{
    public Guid Value { get; private init; }

    /// <summary>
    /// Идентификатор жанра
    /// </summary>
    /// <param name="id">Идентификатор</param>
    public GenreId(Guid id)
    {
        Value = id;
    }
}