using RtfmAPI.Domain.Models.Bands.ValueObjects;

namespace RtfmAPI.Domain.Models.Bands.Events;

/// <summary>
/// Событие изменения названия музыкальной группы.
/// </summary>
/// <param name="Band">Музыкальная группа.</param>
/// <param name="Name">Новое название музыкальной группы.</param>
public record BandNameChangedDomainEvent(Band Band, BandName Name) : IBandDomainEvent;