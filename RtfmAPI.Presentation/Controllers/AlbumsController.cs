using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RftmAPI.Domain.Models.Albums;
using RtfmAPI.Application.Requests.Albums.Commands.AddAlbum;
using RtfmAPI.Application.Requests.Albums.Queries.GetAlbumById;
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
    
    /// <summary>
    /// Получение музыкального альбома по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Музыкальный альбом</returns>
    [HttpGet("{id}")]
    public Task<Album?> GetAlbumByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var query = new GetAlbumByIdQuery
        {
            Id = id
        };

        return Mediator.Send(query, cancellationToken);
    }
    
    /// <summary>
    /// Добавление музыкального альбома
    /// </summary>
    /// <param name="name">Название музыкального альбома</param>
    /// <param name="releaseDate">Дата выпуска</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Добавленный музыкальный альбом</returns>
    [HttpPost]
    public Task<Album> AddAlbumAsync([FromQuery] string name, [FromQuery] DateTime releaseDate, CancellationToken cancellationToken = default)
    {
        var command = new AddAlbumCommand
        {
            Name = name,
            ReleaseDate = releaseDate
        };

        return Mediator.Send(command, cancellationToken);
    }
}