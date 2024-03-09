using System.Collections.Generic;
using RtfmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Domain.Models.Tracks.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Albums.Events;

/// <summary>
/// Событие добавления музыкальных треков к музыкальному альбому.
/// </summary>
/// <param name="AlbumId">Идентификатор музыкального альбома.</param>
/// <param name="AddedTrackIds">Идентификаторы добавленных музыкальных треков.</param>
public record TracksAddedToAlbumDomainEvent(AlbumId AlbumId, IReadOnlyCollection<TrackId> AddedTrackIds) : IDomainEvent;