using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Aggregates.Albums.ValueObjects;

/// <summary>
/// Идентификатор альбома
/// </summary>
public sealed class AlbumId : AggregateRootId<Guid>
{
    /// <summary>
    /// Значение идентификатора
    /// </summary>
    public override Guid Value { get; }

    /// <summary>
    /// Идентификатор альбома
    /// </summary>
    private AlbumId()
    {
        
    }
    
    /// <summary>
    /// Идентификатор альбома
    /// </summary>
    /// <param name="value"></param>
    private AlbumId(Guid value)
    {
        Value = value;
    }
    
    /// <summary>
    /// Создание идентификатора альбома
    /// </summary>
    /// <returns>Идентификатор альбома</returns>
    public static AlbumId Create()
    {
        return new AlbumId(Guid.NewGuid());
    }

    /// <summary>
    /// Создание идентификатора альбома
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <returns>Идентификатор альбомаы</returns>
    public static AlbumId Create(Guid id)
    {
        return new AlbumId(id);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}