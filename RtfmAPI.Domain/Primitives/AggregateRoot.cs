namespace RftmAPI.Domain.Primitives;

/// <summary>
/// Примитив агрегата
/// </summary>
public abstract class AggregateRoot<TId, TIdType> : Entity<TId> where TId : AggregateRootId<TIdType>
{
    /// <summary>
    /// Идентификатор агрената
    /// </summary>
    public new AggregateRootId<TIdType> Id { get; }

    /// <summary>
    /// Примитив агрегата
    /// </summary>
    /// <param name="id">Идентификатор агрената</param>
    protected AggregateRoot(TId id)
    {
        Id = id;
    }

    /// <summary>
    /// Для EF Core
    /// </summary>
#pragma warning disable CS8618
    protected AggregateRoot()
    {
    }
#pragma warning restore CS8618
}