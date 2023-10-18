using RftmAPI.Domain.Aggregates.Band.ValueObjects;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Aggregates.Band;

/// <summary>
/// Музыкальная группа.
/// </summary>
public sealed class Band : AggregateRoot<BandId, Guid>
{
    /// <summary>
    /// Название музыкальной группы.
    /// </summary>
    public BandName Name { get; private set; }

    /// <summary>
    /// Музыкальная группа.
    /// </summary>
#pragma warning disable CS8618
    private Band()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Музыкальная группа.
    /// </summary>
    /// <param name="name">Название музыкальной группы.</param>
    public Band(string name) : base(BandId.Create())
    {
        Name = new BandName(name);
    }
}