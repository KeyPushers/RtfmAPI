using System;
using MediatR;
using RftmAPI.Domain.Primitives;

namespace RtfmAPI.Application.Requests.Albums.Commands.DeleteAlbumById;

/// <summary>
/// Команда удаления альбома.
/// </summary>
public class DeleteAlbumByIdCommand : IRequest<Result<Unit>>
{
    /// <summary>
    /// Идентификатор альбома.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Создание команды удаления альбома.
    /// </summary>
    /// <param name="id">Идентификатор альбома.</param>
    public DeleteAlbumByIdCommand(Guid id)
    {
        Id = id;
    }
}