using System;
using System.Collections.Generic;
namespace RtfmAPI.Application.Requests.Bands.Commands.ModifyBand.Dtos;

/// <summary>
/// Объект переноса данных команды изменения музыкальной группы.
/// </summary>
public sealed class ModifyingBand
{
    /// <summary>
    /// Название музыкальной группы.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Идентификаторы добавляемых музыкальных альбомов.
    /// </summary>
    public List<Guid>? AddingAlbumsIds { get; init; }

    /// <summary>
    /// Идентификаторы добавляемых музыкальных жанров.
    /// </summary>
    public List<Guid>? AddingGenresIds { get; init; }

    /// <summary>
    /// Идентификаторы удаляемых музыкальных альбомов.
    /// </summary>
    public List<Guid>? RemovingAlbumsIds { get; init; }

    /// <summary>
    /// Идентификаторы удаляемых музыкальных жанров.
    /// </summary>
    public List<Guid>? RemovingGenresIds { get; init; }
}