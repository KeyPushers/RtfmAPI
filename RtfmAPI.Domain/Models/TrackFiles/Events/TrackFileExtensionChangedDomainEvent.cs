using RtfmAPI.Domain.Models.TrackFiles.ValueObjects;

namespace RtfmAPI.Domain.Models.TrackFiles.Events;

/// <summary>
/// Событие изменения расширения файла музыкального трека.
/// </summary>
/// <param name="TrackFileId">Идентификатор файла музыкального трека.</param>
/// <param name="TrackFileExtension">Расширение файла музыкального трека.</param>
public record TrackFileExtensionChangedDomainEvent
    (TrackFileId TrackFileId, TrackFileExtension TrackFileExtension) : ITrackFileDomainEvent;