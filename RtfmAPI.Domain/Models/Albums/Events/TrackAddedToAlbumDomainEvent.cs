using RftmAPI.Domain.Models.Tracks;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Albums.Events;

/// <summary>
/// Событие добавления музыкального альбома в музыкальный трек.
/// </summary>
/// <param name="Album">Музыкальный альбом.</param>
/// <param name="Track">Музыкальный трека.</param>
public record TrackAddedToAlbumDomainEvent(Album Album, Track Track) : IDomainEvent;