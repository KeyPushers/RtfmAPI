using System.Collections.Generic;
using RtfmAPI.Domain.Models.Genres.ValueObjects;

namespace RtfmAPI.Domain.Models.Bands.Events;

/// <summary>
/// Событие добавления музыкальных жанров к музыкальной группе.
/// </summary>
/// <param name="Band">Музыкальная группа.</param>
/// <param name="AddedGenreIds">Идентификаторы добавленных музыкальных жанров.</param>
public record GenresAddedToBandDomainEvent(Band Band, IReadOnlyCollection<GenreId> AddedGenreIds) : IBandDomainEvent;