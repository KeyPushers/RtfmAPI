using RftmAPI.Domain.Models.Albums;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Bands.Events;

/// <summary>
/// Событие удаления музыкальных альбомов из музыкальной группы.
/// </summary>
/// <param name="Band">Музыкальная группа.</param>
/// <param name="RemovedAlbums">Удаленные музыкальные альбомы.</param>
public record AlbumsRemovedFromBandDomainEvent(Band Band, IReadOnlyCollection<Album> RemovedAlbums) : IDomainEvent;