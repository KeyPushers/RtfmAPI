namespace RftmAPI.Domain.Primitives;

/// <summary>
/// Примитив сущности
/// </summary>
public abstract class Entity
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; private init; }

    /// <summary>
    /// Базовая сущность
    /// </summary>
    protected Entity()
    {
        Id = Guid.NewGuid();
    }
}