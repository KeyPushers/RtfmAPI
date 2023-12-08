using RftmAPI.Domain.Models.Bands;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Albums.Events;

/// <summary>
/// Событие добавления музыкальных групп к музыкальному альбому.
/// </summary>
/// <param name="Album">Музыкальный альбом.</param>
/// <param name="AddedBands">Добавленные музыкальные группы.</param>
public record BandsAddedToAlbumDomainEvent(Album Album, IReadOnlyCollection<Band> AddedBands) : IDomainEvent;