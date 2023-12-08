using System;
using System.Collections.Generic;

namespace RtfmAPI.Application.Requests.Albums.Commands.ModifyAlbum.Dtos;

/// <summary>
/// Объект переноса данных команды изменения музыкального альбома.
/// </summary>
public sealed class ModifyingAlbum
{
    /// <summary>
    /// Название музыкального альбома.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Дата выпуска музыкального альбома.
    /// </summary>
    public DateTime? ReleaseDate { get; init; }
    
    /// <summary>
    /// Идентификаторы добавляемых музыкальных треков.
    /// </summary>
    public List<Guid>? AddingTracksIds { get; init; }

    /// <summary>
    /// Идентификаторы добавляемых музыкальных групп.
    /// </summary>
    public List<Guid>? AddingBandsIds { get; init; }

    /// <summary>
    /// Идентификаторы удаляемых музыкальных треков.
    /// </summary>
    public List<Guid>? RemovingTracksIds { get; init; }

    /// <summary>
    /// Идентификаторы удаляемых музыкальных групп.
    /// </summary>
    public List<Guid>? RemovingBandsIds { get; init; }
}