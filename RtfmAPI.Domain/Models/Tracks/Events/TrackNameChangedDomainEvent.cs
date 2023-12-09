using RftmAPI.Domain.Models.Tracks.ValueObjects;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Tracks.Events;

/// <summary>
/// Событие изменения названия музыкального трека.
/// </summary>
/// <param name="Track">Музыкальный трек.</param>
/// <param name="Name">Новое название музыкального трека.</param>
public record TrackNameChangedDomainEvent(Track Track, TrackName Name) : IDomainEvent;