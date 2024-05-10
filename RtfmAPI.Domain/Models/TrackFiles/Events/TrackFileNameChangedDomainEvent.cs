﻿using RtfmAPI.Domain.Models.TrackFiles.ValueObjects;

namespace RtfmAPI.Domain.Models.TrackFiles.Events;

/// <summary>
/// Событие изменения названия файла музыкального трека.
/// </summary>
/// <param name="TrackFileId">Идентификатора файла музыкального трека.</param>
/// <param name="TrackFileName">Название файла музыкального трека.</param>
public record TrackFileNameChangedDomainEvent
    (TrackFileId TrackFileId, TrackFileName TrackFileName) : ITrackFileDomainEvent;