using RtfmAPI.Domain.Models.TrackFiles.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.TrackFiles.Events;

/// <summary>
/// Событие удаления музыкального трека.
/// </summary>
/// <param name="TrackFileId">Идентификатор файла музыкального трека.</param>
public record TrackFileDeletedDomainEvent(TrackFileId TrackFileId) : IDomainEvent;