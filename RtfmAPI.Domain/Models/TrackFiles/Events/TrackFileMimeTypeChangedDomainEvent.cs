using RtfmAPI.Domain.Models.TrackFiles.ValueObjects;

namespace RtfmAPI.Domain.Models.TrackFiles.Events;

/// <summary>
/// Событие изменения MIME-типа файла музыкального трека.
/// </summary>
/// <param name="TrackFileId">Идентификатор файла музыкального трека.</param>
/// <param name="TrackFileMimeType">MIME-тип файла музыкального трека.</param>
public record TrackFileMimeTypeChangedDomainEvent
    (TrackFileId TrackFileId, TrackFileMimeType TrackFileMimeType) : ITrackFileDomainEvent;