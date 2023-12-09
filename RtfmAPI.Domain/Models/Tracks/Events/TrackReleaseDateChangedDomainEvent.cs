using RftmAPI.Domain.Models.Tracks.ValueObjects;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Tracks.Events;

/// <summary>
/// Событие изменения даты выпуска музыкального трека.
/// </summary>
/// <param name="Track">Музыкальный трек.</param>
/// <param name="ReleaseDate">Дата выпуска музыкального трека.</param>
public record TrackReleaseDateChangedDomainEvent(Track Track, TrackReleaseDate ReleaseDate) : IDomainEvent;