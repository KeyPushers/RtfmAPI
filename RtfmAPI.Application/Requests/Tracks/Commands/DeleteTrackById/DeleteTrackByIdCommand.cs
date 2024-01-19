using System;
using MediatR;
using RftmAPI.Domain.Primitives;

namespace RtfmAPI.Application.Requests.Tracks.Commands.DeleteTrackById;

/// <summary>
/// Команда удаления музыкального трека по идентификатору.
/// </summary>
public class DeleteTrackByIdCommand : IRequest<Result<Unit>>
{
    /// <summary>
    /// Идентификатор музыкального трека.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Создание команды удаления музыкального трека по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор музыкального трека.</param>
    public DeleteTrackByIdCommand(Guid id)
    {
        Id = id;
    }
}