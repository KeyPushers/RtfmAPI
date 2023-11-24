using RftmAPI.Domain.Models.Albums;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Bands.Events;

/// <summary>
/// Событие добавления музыкальных альбомов в музыкальную группу.
/// </summary>
/// <param name="Band">Музыкальная группа.</param>
/// <param name="AddedAlbums">Добавленные музыкальные альбомы.</param>
public record AlbumsAddedToBandDomainEvent(Band Band, IReadOnlyCollection<Album> AddedAlbums) : IDomainEvent;