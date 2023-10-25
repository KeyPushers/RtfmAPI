namespace RftmAPI.Domain.Primitives;

/// <summary>
/// Примитив сущности.
/// </summary>
public abstract class Entity<TId>
{
    /// <summary>
    /// Идентификатор сущности.
    /// </summary>
    public TId Id { get; protected init; }
    
    /// <summary>
    /// Создание примитива сущности.
    /// </summary>
    /// <remarks>Требуется для EF Core.</remarks>
#pragma warning disable CS8618
    protected Entity()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Создание примитива сущности.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    protected Entity(TId id)
    {
        Id = id;
    }
}