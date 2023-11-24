using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RtfmAPI.Application.Requests.Bands.Commands.AddBand;
using RtfmAPI.Application.Requests.Bands.Commands.AddBand.Dtos;
using RtfmAPI.Application.Requests.Bands.Commands.ModifyBand;
using RtfmAPI.Application.Requests.Bands.Commands.ModifyBand.Dtos;
using RtfmAPI.Application.Requests.Bands.Queries.GetBandInfo;
using RtfmAPI.Application.Requests.Bands.Queries.GetBandInfo.Dtos;
using RtfmAPI.Application.Requests.Bands.Queries.GetBands;
using RtfmAPI.Application.Requests.Bands.Queries.GetBands.Dtos;

namespace RtfmAPI.Presentation.Controllers;

/// <summary>
/// Контроллер музыкальных групп.
/// </summary>
public class BandsController : ApiControllerBase
{
    /// <summary>
    /// Создание контроллера музыкальных групп.
    /// </summary>
    /// <param name="mediator">Медиатор.</param>
    public BandsController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Получение музыкальных групп.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Музыкальные группы.</returns>
    [HttpGet]
    public async Task<ActionResult<BandItems>> GetBandsAsync(CancellationToken cancellationToken = default)
    {
        var queryResult = await Mediator.Send(new GetBandsQuery(), cancellationToken);
        if (queryResult.IsFailed)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, queryResult.Error);
        }

        return Ok(queryResult.Value);
    }

    /// <summary>
    /// Получение музыкального группы по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Музыкальная группа.</returns>
    [HttpGet("{id:guid}", Name = nameof(GetBandInfoAsync))]
    public async Task<ActionResult<BandInfo>> GetBandInfoAsync([FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var queryResult = await Mediator.Send(new GetBandInfoQuery(id), cancellationToken);
        if (queryResult.IsFailed)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, queryResult.Error);
        }

        return Ok(queryResult.Value);
    }

    /// <summary>
    /// Добавление музыкальной группы.
    /// </summary>
    /// <param name="request">Объект переноса данных добавляемой музыкальной группы.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Добавленная музыкальная группа.</returns>
    [HttpPost]
    public async Task<ActionResult<AddedBand>> AddBandAsync([FromBody] AddingBand request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(nameof(request.Name));
        }

        var commandResult = await Mediator.Send(new AddBandCommand(request.Name), cancellationToken);
        if (commandResult.IsFailed)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, commandResult.Error);
        }

        return CreatedAtRoute(nameof(GetBandInfoAsync), new {commandResult.Value.Name}, commandResult.Value);
    }

    /// <summary>
    /// Изменение музыкальной группы.
    /// </summary>
    /// <param name="id">Идентификатор музыкальной группы.</param>
    /// <param name="request">Объект переноса данных команды изменения музыкальной группы.</param>
    /// <param name="cancellationToken">Токен отмены</param>
    [HttpPost("{id:guid}/modify")]
    public async Task<ActionResult> ModifyBandAsync([FromRoute] Guid id, [FromBody] ModifyingBand request,
        CancellationToken cancellationToken = default)
    {
        var command = new ModifyBandCommand
        {
            BandId = id,
            Name = request.Name,
            AddingAlbumsIds = request.AddingAlbumsIds,
            AddingGenresIds = request.AddingGenresIds,
            RemovingAlbumsIds = request.RemovingAlbumsIds,
            RemovingGenresIds = request.RemovingGenresIds
        };

        var commandResult = await Mediator.Send(command, cancellationToken);
        if (commandResult.IsFailed)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, commandResult.Error);
        }

        return Ok();
    }
}