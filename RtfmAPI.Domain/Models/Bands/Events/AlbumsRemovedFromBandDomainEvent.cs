using System.Collections.Generic;
using RtfmAPI.Domain.Models.Albums.ValueObjects;

namespace RtfmAPI.Domain.Models.Bands.Events;

/// <summary>
/// Событие удаления музыкальных альбомов из музыкальной группы.
/// </summary>
/// <param name="Band">Музыкальная группа.</param>
/// <param name="RemovedAlbumIds">Идентификаторы удаленных музыкальных альбомов.</param>
public record AlbumsRemovedFromBandDomainEvent
    (Band Band, IReadOnlyCollection<AlbumId> RemovedAlbumIds) : IBandDomainEvent;