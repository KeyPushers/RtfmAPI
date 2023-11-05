using RftmAPI.Domain.Models.Albums;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Tracks.Events;

/// <summary>
/// Событие добавления музыкального альбома в музыкальный трек.
/// </summary>
/// <param name="Track">Музыкальный трек.</param>
/// <param name="Album">Музыкальный альбом.</param>
public record AlbumAddedToTrackDomainEvent(Track Track, Album Album) : IDomainEvent;