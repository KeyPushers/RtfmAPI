using RtfmAPI.Domain.Models.TrackFiles.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.TrackFiles.Events;

/// <summary>
/// Событие изменения содержимого файла музыкального трека.
/// </summary>
/// <param name="TrackFileId">Идентификатор файла музыкального трека.</param>
/// <param name="TrackFileData">Содержимое файла музыкального трека.</param>
public record TrackFileDataChangedDomainEvent(TrackFileId TrackFileId, TrackFileData TrackFileData) : IDomainEvent;