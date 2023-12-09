using RftmAPI.Domain.Models.Genres;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Tracks.Events;

/// <summary>
/// Событие добавления музыкальных жанров к музыкальной группе.
/// </summary>
/// <param name="Track">Музыкальная трек.</param>
/// <param name="AddedGenres">Добавленные музыкальные жанры.</param>
public record GenresAddedToTrackDomainEvent(Track Track, IReadOnlyCollection<Genre> AddedGenres) : IDomainEvent;