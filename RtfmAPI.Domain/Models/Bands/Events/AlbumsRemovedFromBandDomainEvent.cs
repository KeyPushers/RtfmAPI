using System.Collections.Generic;
using RtfmAPI.Domain.Models.Albums;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Bands.Events;

/// <summary>
/// Событие удаления музыкальных альбомов из музыкальной группы.
/// </summary>
/// <param name="Band">Музыкальная группа.</param>
/// <param name="RemovedAlbums">Удаленные музыкальные альбомы.</param>
public record AlbumsRemovedFromBandDomainEvent(Band Band, IReadOnlyCollection<Album> RemovedAlbums) : IDomainEvent;