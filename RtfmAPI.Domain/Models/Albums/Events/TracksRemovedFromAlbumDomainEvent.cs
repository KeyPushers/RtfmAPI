using System.Collections.Generic;
using RtfmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Domain.Models.Tracks.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Albums.Events;

/// <summary>
/// Событие удаления музыкальных треков из музыкального альбома.
/// </summary>
/// <param name="AlbumId">Идентификатор музыкального альбома.</param>
/// <param name="RemovedTrackIds">Идентификаторы удаленных музыкальных треков.</param>
public record TracksRemovedFromAlbumDomainEvent(AlbumId AlbumId, IReadOnlyCollection<TrackId> RemovedTrackIds) : IDomainEvent;