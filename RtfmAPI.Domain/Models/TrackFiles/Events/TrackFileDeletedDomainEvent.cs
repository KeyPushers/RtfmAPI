using RftmAPI.Domain.Models.TrackFiles.ValueObjects;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.TrackFiles.Events;

/// <summary>
/// Событие удаления музыкального трека.
/// </summary>
/// <param name="TrackFileId">Идентификатор файла музыкального трека.</param>
public record TrackFileDeletedDomainEvent(TrackFileId TrackFileId) : IDomainEvent;