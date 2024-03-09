using RtfmAPI.Domain.Models.Tracks.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Tracks.Events;

/// <summary>
/// Событие изменения даты выпуска музыкального трека.
/// </summary>
/// <param name="TrackId">Идентификатор музыкального трека.</param>
/// <param name="ReleaseDate">Дата выпуска музыкального трека.</param>
public record TrackReleaseDateChangedDomainEvent(TrackId TrackId, TrackReleaseDate ReleaseDate) : IDomainEvent;