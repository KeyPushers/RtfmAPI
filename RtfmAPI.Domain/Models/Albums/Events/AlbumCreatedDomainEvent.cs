namespace RtfmAPI.Domain.Models.Albums.Events;

/// <summary>
/// Событие удаления музыкального альбома.
/// </summary>
/// <param name="Album">Созданный музыкальный альбом.</param>
public record AlbumCreatedDomainEvent(Album Album) : IAlbumDomainEvent;