using RtfmAPI.Domain.Models.Tracks.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Tracks.Events;

/// <summary>
/// Событие удаления музыкального трека.
/// </summary>
/// <param name="TrackId">Идентификатор музыкального трека.</param>
public record TrackDeletedDomainEvent(TrackId TrackId) : IDomainEvent;