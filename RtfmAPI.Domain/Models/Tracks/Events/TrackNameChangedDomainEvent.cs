using RtfmAPI.Domain.Models.Tracks.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Tracks.Events;

/// <summary>
/// Событие изменения названия музыкального трека.
/// </summary>
/// <param name="TrackId">Идентификатор музыкального трека.</param>
/// <param name="Name">Новое название музыкального трека.</param>
public record TrackNameChangedDomainEvent(TrackId TrackId, TrackName Name) : IDomainEvent;