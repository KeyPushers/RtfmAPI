using System.Collections.Generic;
using RtfmAPI.Domain.Models.Genres.ValueObjects;
using RtfmAPI.Domain.Models.Tracks.ValueObjects;

namespace RtfmAPI.Domain.Models.Tracks.Events;

/// <summary>
/// Событие удаления музыкальных альбомов из музыкальной группы.
/// </summary>
/// <param name="TrackId">Идентификатор музыкального трека.</param>
/// <param name="RemovedGenreIds">Идентификаторы удаленных музыкальных жанров.</param>
public record GenresRemovedFromTrackDomainEvent(TrackId TrackId, IReadOnlyCollection<GenreId> RemovedGenreIds) : ITrackDomainEvent;