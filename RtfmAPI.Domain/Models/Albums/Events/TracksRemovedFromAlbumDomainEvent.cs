using RftmAPI.Domain.Models.Tracks;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Albums.Events;

/// <summary>
/// Событие удаления музыкальных треков из музыкального альбома.
/// </summary>
/// <param name="Album">Музыкальный альбом.</param>
/// <param name="RemovedTracks">Удаленные музыкальные треки.</param>
public record TracksRemovedFromAlbumDomainEvent(Album Album, IReadOnlyCollection<Track> RemovedTracks) : IDomainEvent;