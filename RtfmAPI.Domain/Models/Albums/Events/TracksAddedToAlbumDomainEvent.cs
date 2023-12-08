using RftmAPI.Domain.Models.Tracks;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Albums.Events;

/// <summary>
/// Событие добавления музыкальных треков к музыкальному альбому.
/// </summary>
/// <param name="Album">Музыкальный альбом.</param>
/// <param name="AddedTracks">Добавленные музыкальные треки.</param>
public record TracksAddedToAlbumDomainEvent(Album Album, IReadOnlyCollection<Track> AddedTracks) : IDomainEvent;