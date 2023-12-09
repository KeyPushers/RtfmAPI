using RftmAPI.Domain.Models.Genres;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Tracks.Events;

/// <summary>
/// Событие удаления музыкальных альбомов из музыкальной группы.
/// </summary>
/// <param name="Track">Музыкальная трек.</param>
/// <param name="RemovedGenres">Удаленные музыкальные жанры.</param>
public record GenresRemovedFromTrackDomainEvent(Track Track, IReadOnlyCollection<Genre> RemovedGenres) : IDomainEvent;