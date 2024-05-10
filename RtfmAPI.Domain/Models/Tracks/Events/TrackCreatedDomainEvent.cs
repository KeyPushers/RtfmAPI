using RtfmAPI.Domain.Models.Tracks.ValueObjects;

namespace RtfmAPI.Domain.Models.Tracks.Events;

/// <summary>
/// Событие создания музыкального трека.
/// </summary>
/// <param name="TrackId">Идентификатор добавленного музыкального трека.</param>
public record TrackCreatedDomainEvent(TrackId TrackId) : ITrackDomainEvent;