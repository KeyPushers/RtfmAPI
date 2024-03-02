using System;
using System.Collections.Generic;
using MediatR;
using RftmAPI.Domain.Primitives;

namespace RtfmAPI.Application.Requests.Albums.Commands.ModifyAlbum;

/// <summary>
/// Команда изменения музыкального альбома.
/// </summary>
public class ModifyAlbumCommand : IRequest<BaseResult>
{
    /// <summary>
    /// Идентификатор музыкального альбома.
    /// </summary>
    public Guid AlbumId { get; init; }

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
    public IReadOnlyCollection<Guid>? AddingTracksIds { get; init; }

    /// <summary>
    /// Идентификаторы удаляемых музыкальных треков.
    /// </summary>
    public IReadOnlyCollection<Guid>? RemovingTracksIds { get; init; }
}