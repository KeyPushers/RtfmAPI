using System.Collections.Generic;
using RtfmAPI.Domain.Models.Genres.ValueObjects;
using RtfmAPI.Domain.Models.Tracks.ValueObjects;

namespace RtfmAPI.Domain.Models.Tracks.Events;

/// <summary>
/// Событие добавления музыкальных жанров к музыкальной группе.
/// </summary>
/// <param name="TrackId">Идентификатор музыкального трека.</param>
/// <param name="AddedGenreIds">Идентификаторы добавленных музыкальных жанров.</param>
public record GenresAddedToTrackDomainEvent
    (TrackId TrackId, IReadOnlyCollection<GenreId> AddedGenreIds) : ITrackDomainEvent;