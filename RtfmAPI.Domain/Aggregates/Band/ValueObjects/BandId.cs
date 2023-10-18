using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Aggregates.Band.ValueObjects;

/// <summary>
/// Идентификатор музыкальной группы.
/// </summary>
public sealed class BandId : AggregateRootId<Guid>
{
    public override Guid Value { get; }

    /// <summary>
    /// Идентификатор музыкального трека
    /// </summary>
    private BandId()
    {
        
    }
    
    /// <summary>
    /// Идентификатор музыкального трека
    /// </summary>
    /// <param name="value">Значение идентификатор</param>
    private BandId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Создание идентификатора музыкального трека
    /// </summary>
    /// <returns>Идентификатор музыкального трека</returns>
    public static BandId Create()
    {
        return new BandId(Guid.NewGuid());
    }

    /// <summary>
    /// Создание идентификатора музыкального трека
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <returns>Идентификатор музыкального трека</returns>
    public static BandId Create(Guid id)
    {
        return new BandId(id);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}