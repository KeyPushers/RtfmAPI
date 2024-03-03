using RtfmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Albums.Events;

/// <summary>
/// Событие изменения даты выпуска музыкального альбома.
/// </summary>
/// <param name="Album">Музыкальный альбом.</param>
/// <param name="ReleaseDate">Дата выпуска музыкального альбома.</param>
public record AlbumReleaseDateChangedDomainEvent(Album Album, AlbumReleaseDate ReleaseDate) : IDomainEvent;