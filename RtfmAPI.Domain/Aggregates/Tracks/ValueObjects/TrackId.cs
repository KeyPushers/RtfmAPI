using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Aggregates.Tracks.ValueObjects;

/// <summary>
/// Идентификатор музыкального трека
/// </summary>
public sealed class TrackId : AggregateRootId<Guid>
{
    public override Guid Value { get; }

    /// <summary>
    /// Идентификатор музыкального трека
    /// </summary>
    private TrackId()
    {
        
    }
    
    /// <summary>
    /// Идентификатор музыкального трека
    /// </summary>
    /// <param name="value">Значение идентификатор</param>
    private TrackId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Создание идентификатора музыкального трека
    /// </summary>
    /// <returns>Идентификатор музыкального трека</returns>
    public static TrackId Create()
    {
        return new TrackId(Guid.NewGuid());
    }

    /// <summary>
    /// Создание идентификатора музыкального трека
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <returns>Идентификатор музыкального трека</returns>
    public static TrackId Create(Guid id)
    {
        return new TrackId(id);
    }
}