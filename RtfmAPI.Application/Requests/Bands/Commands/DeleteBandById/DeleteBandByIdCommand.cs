using System;
using MediatR;
using RftmAPI.Domain.Primitives;

namespace RtfmAPI.Application.Requests.Bands.Commands.DeleteBandById;

/// <summary>
/// Команда удаления музыкальной группы по идентификатору.
/// </summary>
public class DeleteBandByIdCommand : IRequest<Result<Unit>>
{
    /// <summary>
    /// Идентификатор музыкальной группы.
    /// </summary>
    public Guid Id { get; }
    
    /// <summary>
    /// Удаление музыкальной группы по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор музыкальной группы.</param>
    public DeleteBandByIdCommand(Guid id)
    {
        Id = id;
    }
}