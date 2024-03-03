using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RtfmAPI.Application.Requests.Bands.Commands.AddBand;
using RtfmAPI.Application.Requests.Bands.Commands.AddBand.Dtos;
using RtfmAPI.Application.Requests.Bands.Queries.GetBandInfo;
using RtfmAPI.Application.Requests.Bands.Queries.GetBandInfo.Dtos;

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

        return CreatedAtRoute(nameof(GetBandInfoAsync), new {commandResult.Value.Id}, commandResult.Value);
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
}