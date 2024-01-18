using RftmAPI.Domain.Models.Bands.ValueObjects;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Bands.Events;

/// <summary>
/// Событие удаления музыкальной группы.
/// </summary>
/// <param name="BandId">Идентификатор музыкальной группы.</param>
public record BandDeletedDomainEvent(BandId BandId) : IDomainEvent;