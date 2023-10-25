using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RftmAPI.Domain.Models.Bands;
using RtfmAPI.Application.Requests.Bands.Commands.AddBand;
using RtfmAPI.Application.Requests.Bands.Queries.GetBandById;
using RtfmAPI.Application.Requests.Bands.Queries.GetBands;

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
    public Task<List<Band>> GetBandsAsync(CancellationToken cancellationToken = default)
    {
        var query = new GetBandsQuery();

        return Mediator.Send(query, cancellationToken);
    }

    /// <summary>
    /// Получение музыкального группы по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Музыкальная группа.</returns>
    [HttpGet("{id}")]
    public Task<Band?> GetBandByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var query = new GetBandByIdQuery
        {
            Id = id
        };

        return Mediator.Send(query, cancellationToken);
    }

    /// <summary>
    /// Добавление музыкальной группы.
    /// </summary>
    /// <param name="name">Название музыкальной группы.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Добавленная музыкальная группа.</returns>
    [HttpPost]
    public Task<Band> AddBandAsync([FromQuery] string name, CancellationToken cancellationToken = default)
    {
        var command = new AddBandCommand
        {
            Name = name
        };

        return Mediator.Send(command, cancellationToken);
    }
}