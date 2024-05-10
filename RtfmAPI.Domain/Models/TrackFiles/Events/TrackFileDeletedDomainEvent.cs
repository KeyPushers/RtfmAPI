using RtfmAPI.Domain.Models.TrackFiles.ValueObjects;

namespace RtfmAPI.Domain.Models.TrackFiles.Events;

/// <summary>
/// Событие удаления музыкального трека.
/// </summary>
/// <param name="TrackFileId">Идентификатор файла музыкального трека.</param>
public record TrackFileDeletedDomainEvent(TrackFileId TrackFileId) : ITrackFileDomainEvent;