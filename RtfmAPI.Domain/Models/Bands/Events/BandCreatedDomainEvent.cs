using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Bands.Events;

/// <summary>
/// Событие создания музыкальной группы.
/// </summary>
/// <param name="Band">Созданная музыкальная группа.</param>
public record BandCreatedDomainEvent(Band Band) : IDomainEvent;