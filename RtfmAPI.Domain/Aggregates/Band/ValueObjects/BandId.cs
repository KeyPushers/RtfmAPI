using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Aggregates.Band.ValueObjects;

/// <summary>
/// Идентификатор музыкальной группы.
/// </summary>
public sealed class BandId : AggregateRootId<Guid>
{
    /// <summary>
    /// Значение идентификатора.
    /// </summary>
    public override Guid Value { get; }

    /// <summary>
    /// Идентификатор музыкальной группы.
    /// </summary>
    private BandId()
    {
        
    }
    
    /// <summary>
    /// Идентификатор музыкальной группы.
    /// </summary>
    /// <param name="value">Значение идентификатор</param>
    private BandId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Создание идентификатора музыкальной группы.
    /// </summary>
    /// <returns>Идентификатор музыкальной группы</returns>
    public static BandId Create()
    {
        return new BandId(Guid.NewGuid());
    }

    /// <summary>
    /// Создание идентификатора музыкальной группы.
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <returns>Идентификатор музыкальной группы</returns>
    public static BandId Create(Guid id)
    {
        return new BandId(id);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}