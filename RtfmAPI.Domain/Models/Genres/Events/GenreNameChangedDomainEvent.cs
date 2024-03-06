using RtfmAPI.Domain.Models.Genres.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Genres.Events;

/// <summary>
/// Событие изменения названия музыкальной группы.
/// </summary>
/// <param name="Genre">Музыкальный жанр.</param>
/// <param name="Name">Новое название музыкальной группы.</param>
public record GenreNameChangedDomainEvent(Genre Genre, GenreName Name) : IDomainEvent;