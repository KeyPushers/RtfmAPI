using RftmAPI.Domain.Models.Tracks.ValueObjects;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Tracks.Events;

/// <summary>
/// Событие удаления музыкального трека.
/// </summary>
/// <param name="TrackId">Идентификатор музыкального трека.</param>
public record TrackDeletedDomainEvent(TrackId TrackId) : IDomainEvent;