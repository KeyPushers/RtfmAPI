using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RftmAPI.Domain.Models.Albums;
using RtfmAPI.Application.Requests.Albums.Queries.GetAlbums;

namespace RtfmAPI.Presentation.Controllers;

/// <summary>
/// Контроллер музыкальных альбомов.
/// </summary>
public class AlbumsController : ApiControllerBase
{
    /// <summary>
    /// Создание контроллера музыкальных альбомов.
    /// </summary>
    /// <param name="mediator">Медиатор.</param>
    public AlbumsController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Получение музыкальных альбомов.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Музыкальные альбомы.</returns>
    [HttpGet]
    public Task<List<Album>> GetAlbumsAsync(CancellationToken cancellationToken = default)
    {
        var query = new GetAlbumsQuery();

        return Mediator.Send(query, cancellationToken);
    }
}