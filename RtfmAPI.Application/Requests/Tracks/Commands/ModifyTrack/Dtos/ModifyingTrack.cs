using System;
using System.Collections.Generic;

namespace RtfmAPI.Application.Requests.Tracks.Commands.ModifyTrack.Dtos;

/// <summary>
/// Объект переноса данных команды изменения музыкального трека.
/// </summary>
public sealed class ModifyingTrack
{
    /// <summary>
    /// Название музыкального трека.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Дата выпуска музыкального трека.
    /// </summary>
    public DateTime? ReleaseDate { get; init; }
    
    /// <summary>
    /// Идентификатор музыкального альбома.
    /// </summary>
    public Guid? AlbumId { get; init; }
    
    /// <summary>
    /// Идентификаторы добавляемых музыкальных жанров.
    /// </summary>
    public List<Guid>? AddingGenresIds { get; init; }
    
    /// <summary>
    /// Идентификаторы удаляемых музыкальных жанров.
    /// </summary>
    public List<Guid>? RemovingGenresIds { get; init; }
}