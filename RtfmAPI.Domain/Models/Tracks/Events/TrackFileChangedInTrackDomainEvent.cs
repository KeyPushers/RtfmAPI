using RftmAPI.Domain.Models.TrackFiles;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Tracks.Events;

/// <summary>
/// Событие изменения файла музыкального трека.
/// </summary>
/// <param name="Track">Музыкальный трек.</param>
/// <param name="TrackFile">Измененный файл музыкального трека.</param>
public record TrackFileChangedInTrackDomainEvent(Track Track, TrackFile TrackFile): IDomainEvent;