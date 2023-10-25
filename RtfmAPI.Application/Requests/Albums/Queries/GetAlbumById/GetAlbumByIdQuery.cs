using System;
using MediatR;
using RftmAPI.Domain.Models.Albums;

namespace RtfmAPI.Application.Requests.Albums.Queries.GetAlbumById;

/// <summary>
/// Запрос музыкального альбома по идентификатору
/// </summary>
public class GetAlbumByIdQuery : IRequest<Album?>
{
    /// <summary>
    /// Идентификатор музыкального альбома
    /// </summary>
    public Guid Id { get; init; }
}