using System.Collections.Generic;
using RtfmAPI.Domain.Models.Genres.ValueObjects;

namespace RtfmAPI.Domain.Models.Bands.Events;

/// <summary>
/// Событие удаления музыкальных альбомов из музыкальной группы.
/// </summary>
/// <param name="Band">Музыкальная группа.</param>
/// <param name="RemovedGenreIds">Идентификаторы удаленных музыкальных жанров.</param>
public record GenresRemovedFromBandDomainEvent
    (Band Band, IReadOnlyCollection<GenreId> RemovedGenreIds) : IBandDomainEvent;