using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Genres.Events;

/// <summary>
/// Событие создания музыкального жанра.
/// </summary>
/// <param name="Genre">Созданный музыкальный жанр.</param>
public record GenreCreatedDomainEvent(Genre Genre) : IDomainEvent;