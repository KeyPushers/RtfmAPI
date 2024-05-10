namespace RtfmAPI.Domain.Models.Bands.Events;

/// <summary>
/// Событие удаления музыкальной группы.
/// </summary>
/// <param name="Band">Удаленная музыкальная группа.</param>
public record BandDeletedDomainEvent(Band Band) : IBandDomainEvent;