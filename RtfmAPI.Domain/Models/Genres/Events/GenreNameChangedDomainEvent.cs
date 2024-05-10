using RtfmAPI.Domain.Models.Genres.ValueObjects;

namespace RtfmAPI.Domain.Models.Genres.Events;

/// <summary>
/// Событие изменения названия музыкального жанра.
/// </summary>
/// <param name="Genre">Музыкальный жанр.</param>
/// <param name="Name">Новое название музыкальной группы.</param>
public record GenreNameChangedDomainEvent(Genre Genre, GenreName Name) : IGenreDomainEvent;