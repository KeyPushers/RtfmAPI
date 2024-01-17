using RftmAPI.Domain.Models.Albums.ValueObjects;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Albums.Events;

/// <summary>
/// Событие удаления музыкального альбома.
/// </summary>
/// <param name="AlbumId">Идентификатор музыкального альбома.</param>
public record AlbumDeletedDomainEvent(AlbumId AlbumId) : IDomainEvent;