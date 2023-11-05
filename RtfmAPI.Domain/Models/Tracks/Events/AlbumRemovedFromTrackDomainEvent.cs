using RftmAPI.Domain.Models.Albums.ValueObjects;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Tracks.Events;

/// <summary>
/// Доменной событие удаление музыкального альбома из музыкального трека.
/// </summary>
/// <param name="Track">Музыкальный трек.</param>
/// <param name="AlbumId">Идентификатор музыкального альбома.</param>
public record AlbumRemovedFromTrackDomainEvent(Track Track, AlbumId AlbumId) : IDomainEvent;