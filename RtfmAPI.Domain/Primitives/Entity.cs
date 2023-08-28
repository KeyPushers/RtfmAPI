namespace RftmAPI.Domain.Primitives;

/// <summary>
/// Примитив сущности
/// </summary>
public abstract class Entity<TId>
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public TId Id { get; protected init; }
    
#pragma warning disable CS8618
    protected Entity()
    {
    }
#pragma warning restore CS8618

}