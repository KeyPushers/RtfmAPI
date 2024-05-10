using RtfmAPI.Domain.Models.TrackFiles.ValueObjects;
using RtfmAPI.Domain.Models.Tracks.ValueObjects;

namespace RtfmAPI.Domain.Models.Tracks.Events;

/// <summary>
/// Событие изменения файла музыкального трека.
/// </summary>
/// <param name="TrackId">Идентификатор музыкального трека.</param>
/// <param name="TrackFileId">Идентификатора измененного файла музыкального трека.</param>
public record TrackFileChangedInTrackDomainEvent(TrackId TrackId, TrackFileId TrackFileId) : ITrackDomainEvent;