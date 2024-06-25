using System;
using System.Collections.Generic;
using MediatR;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Application.Requests.Bands.Commands.ModifyBand;

/// <summary>
/// Команда изменения музыкальной группы.
/// </summary>
public class ModifyBandCommand : IRequest<BaseResult>
{
    /// <summary>
    /// Идентификатор музыкальной группы.
    /// </summary>
    public Guid BandId { get; init; }

    /// <summary>
    /// Название музыкальной группы.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Идентификаторы добавляемых музыкальных альбомов.
    /// </summary>
    public List<Guid> AddingAlbumsIds { get; init; } = new();

    /// <summary>
    /// Идентификаторы удаляемых музыкальных альбомов.
    /// </summary>
    public List<Guid> RemovingAlbumsIds { get; init; } = new();
    
    /// <summary>
    /// Идентификаторы добавляемых музыкальных жанров.
    /// </summary>
    public List<Guid> AddingGenresIds { get; init; } = new();
    
    /// <summary>
    /// Идентификаторы удаляемых музыкальных жанров.
    /// </summary>
    public List<Guid> RemovingGenresIds { get; init; } = new();
}