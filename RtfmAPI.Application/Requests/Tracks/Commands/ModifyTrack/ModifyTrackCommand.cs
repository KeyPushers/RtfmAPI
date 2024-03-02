using System;
using System.Collections.Generic;
using MediatR;
using RftmAPI.Domain.Primitives;

namespace RtfmAPI.Application.Requests.Tracks.Commands.ModifyTrack;

/// <summary>
/// Команда изменения музыкального трека.
/// </summary>
public class ModifyTrackCommand : IRequest<BaseResult>
{
    /// <summary>
    /// Идентификатор музыкального трека.
    /// </summary>
    public Guid TrackId { get; init; }

    /// <summary>
    /// Название музыкального трека.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Дата выпуска музыкального трека.
    /// </summary>
    public DateTime? ReleaseDate { get; init; }
    
    /// <summary>
    /// Идентификаторы добавляемых музыкальных жанров.
    /// </summary>
    public IReadOnlyCollection<Guid>? AddingGenresIds { get; init; }
    
    /// <summary>
    /// Идентификаторы удаляемых музыкальных жанров.
    /// </summary>
    public IReadOnlyCollection<Guid>? RemovingGenresIds { get; init; }
}