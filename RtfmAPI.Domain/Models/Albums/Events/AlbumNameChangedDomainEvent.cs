using RtfmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Albums.Events;

/// <summary>
/// Событие изменения названия музыкального альбома.
/// </summary>
/// <param name="Album">Музыкальный альбом.</param>
/// <param name="Name">Новое название музыкального альбома.</param>
public record AlbumNameChangedDomainEvent(Album Album, AlbumName Name) : IDomainEvent;