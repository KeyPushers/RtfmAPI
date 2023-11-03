using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Tracks.Events;

/// <summary>
/// Событие создания музыкального трека.
/// </summary>
public record TrackCreatedDomainEvent(Track Track) : IDomainEvent;