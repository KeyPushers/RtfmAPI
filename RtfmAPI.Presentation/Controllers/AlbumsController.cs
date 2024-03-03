using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RtfmAPI.Application.Requests.Albums.Commands.AddAlbum;
using RtfmAPI.Application.Requests.Albums.Queries.GetAlbumInfo;
using RtfmAPI.Application.Requests.Albums.Queries.GetAlbumInfo.Dtos;

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
    /// Получение информации о музыкальном альбома по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор музыкального альбома.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Информация о музыкальном альбома.</returns>
    [HttpGet("{id:guid}", Name = nameof(GetAlbumInfoAsync))]
    public async Task<ActionResult<AlbumInfo>> GetAlbumInfoAsync([FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAlbumInfoQuery
        {
            Id = id
        };

        var queryResult = await Mediator.Send(query, cancellationToken);
        if (queryResult.IsFailed)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, queryResult.Error);
        }

        return queryResult.Value;
    }

    /// <summary>
    /// Добавление музыкального альбома
    /// </summary>
    /// <param name="name">Название музыкального альбома</param>
    /// <param name="releaseDate">Дата выпуска</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Добавленный музыкальный альбом</returns>
    [HttpPost]
    public async Task<ActionResult> AddAlbumAsync([FromQuery] string name, [FromQuery] DateTime releaseDate,
        CancellationToken cancellationToken = default)
    {
        var command = new AddAlbumCommand
        {
            Name = name,
            ReleaseDate = releaseDate
        };

        var commandResult = await Mediator.Send(command, cancellationToken);
        if (commandResult.IsFailed)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, commandResult.Error);
        }

        return CreatedAtRoute(nameof(GetAlbumInfoAsync), new {commandResult.Value.Id}, commandResult.Value);
    }
}