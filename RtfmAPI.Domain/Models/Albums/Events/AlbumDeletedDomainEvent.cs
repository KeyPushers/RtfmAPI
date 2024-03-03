using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Albums.Events;

/// <summary>
/// Событие удаления музыкального альбома.
/// </summary>
/// <param name="Album">Удаленный музыкальный альбом.</param>
public record AlbumDeletedDomainEvent(Album Album) : IDomainEvent;