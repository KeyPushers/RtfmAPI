using RtfmAPI.Domain.Models.TrackFiles.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.TrackFiles.Events;

/// <summary>
/// Событие создания файла музыкального трека.
/// </summary>
/// <param name="TrackFileId">Идентификатор созданного файла музыкального трека.</param>
public record TrackFileCreatedDomainEvent(TrackFileId TrackFileId) : IDomainEvent;