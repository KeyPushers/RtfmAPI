using RftmAPI.Domain.Models.Tracks.ValueObjects;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Albums.Events;

/// <summary>
/// Доменное событие удаление музыкального трека из альбома.
/// </summary>
/// <param name="Album">Музыкальный альбом.</param>
/// <param name="TrackId">Музыкальный трек.</param>
public record TrackRemovedFromAlbumDomainEvent(Album Album, TrackId TrackId) : IDomainEvent;