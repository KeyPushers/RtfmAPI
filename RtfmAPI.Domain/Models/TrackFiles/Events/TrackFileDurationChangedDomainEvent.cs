using RtfmAPI.Domain.Models.TrackFiles.ValueObjects;

namespace RtfmAPI.Domain.Models.TrackFiles.Events;

/// <summary>
/// Событие изменения продолжительности файла музыкального трека.
/// </summary>
/// <param name="TrackFileId">Идентификатор файла музыкального трека.</param>
/// <param name="TrackFileDuration">Продолжительность файла музыкального трека.</param>
public record TrackFileDurationChangedDomainEvent
    (TrackFileId TrackFileId, TrackFileDuration TrackFileDuration) : ITrackFileDomainEvent;