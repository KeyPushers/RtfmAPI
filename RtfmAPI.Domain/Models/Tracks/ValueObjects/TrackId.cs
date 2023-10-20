using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Tracks.ValueObjects;

/// <summary>
/// Идентификатор музыкального трека.
/// </summary>
public sealed class TrackId : AggregateRootId<Guid>
{
    /// <summary>
    /// Значение идентификатора.
    /// </summary>
    public override Guid Value { get; }
    
    /// <summary>
    /// Идентификатор музыкального трека.
    /// </summary>
    /// <param name="value">Значение идентификатора.</param>
    private TrackId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Создание идентификатора музыкальной трека.
    /// </summary>
    /// <returns>Идентификатор музыкального трека.</returns>
    public static TrackId Create()
    {
        return new TrackId(Guid.NewGuid());
    }

    /// <summary>
    /// Создание идентификатора музыкальной жанра.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <returns>Идентификатор музыкального трека.</returns>
    public static TrackId Create(Guid id)
    {
        return new TrackId(id);
    }
    
    /// <inheritdoc cref="ValueObject.GetEqualityComponents"/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}