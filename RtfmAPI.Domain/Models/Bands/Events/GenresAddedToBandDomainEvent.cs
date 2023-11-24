using RftmAPI.Domain.Models.Genres;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Bands.Events;

/// <summary>
/// Событие добавления музыкальных жанров к музыкальной группе.
/// </summary>
/// <param name="Band">Музыкальная группа.</param>
/// <param name="AddedGenres">Добавленные музыкальные жанры.</param>
public record GenresAddedToBandDomainEvent(Band Band, IReadOnlyCollection<Genre> AddedGenres) : IDomainEvent;