namespace RftmAPI.Domain.Primitives;

/// <summary>
/// Примитив агрегата.
/// </summary>
public abstract class AggregateRoot<TId, TIdType> : Entity<TId> where TId : AggregateRootId<TIdType>
{
    /// <summary>
    /// Примитив агрегата.
    /// </summary>
    /// <param name="id">Идентификатор агрената</param>
    protected AggregateRoot(TId id)
    {
        Id = id;
    }

    /// <summary>
    /// Примитив агрегата.
    /// </summary>
    /// <remarks>Требуется для EF Core.</remarks>
#pragma warning disable CS8618
    protected AggregateRoot()
    {
    }
#pragma warning restore CS8618
}