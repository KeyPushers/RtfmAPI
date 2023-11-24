using RftmAPI.Domain.Models.Genres;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Bands.Events;

/// <summary>
/// Событие удаления музыкальных альбомов из музыкальной группы.
/// </summary>
/// <param name="Band">Музыкальная группа.</param>
/// <param name="RemovedGenres">Удаленные музыкальные жанры.</param>
public record GenresRemovedFromBandDomainEvent(Band Band, IReadOnlyCollection<Genre> RemovedGenres) : IDomainEvent;