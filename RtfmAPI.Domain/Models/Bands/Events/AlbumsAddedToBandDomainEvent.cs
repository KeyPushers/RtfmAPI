using System.Collections.Generic;
using RtfmAPI.Domain.Models.Albums.ValueObjects;

namespace RtfmAPI.Domain.Models.Bands.Events;

/// <summary>
/// Событие добавления музыкальных альбомов в музыкальную группу.
/// </summary>
/// <param name="Band">Музыкальная группа.</param>
/// <param name="AddedAlbumIds">Идентификаторы добавленных музыкальных альбомов.</param>
public record AlbumsAddedToBandDomainEvent(Band Band, IReadOnlyCollection<AlbumId> AddedAlbumIds) : IBandDomainEvent;