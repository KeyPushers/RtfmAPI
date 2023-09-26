namespace RftmAPI.Domain.Primitives;

/// <summary>
/// Примитив идентификатора агрегата
/// </summary>
/// <typeparam name="TId">Тип идентификатора</typeparam>
public abstract class AggregateRootId<TId> : ValueObject
{
    /// <summary>
    /// Значение идентификатор
    /// </summary>
    public abstract TId Value { get; }
}