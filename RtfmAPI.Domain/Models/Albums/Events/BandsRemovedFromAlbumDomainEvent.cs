using RftmAPI.Domain.Models.Bands;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Albums.Events;

/// <summary>
/// Событие удаления музыкальных групп из музыкального альбома.
/// </summary>
/// <param name="Album">Музыкальный альбом.</param>
/// <param name="RemovedBands">Удаленные музыкальные группы.</param>
public record BandsRemovedFromAlbumDomainEvent(Album Album, IReadOnlyCollection<Band> RemovedBands) : IDomainEvent;