using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RftmAPI.Domain.Aggregates.Tracks;
using RtfmAPI.Application.Requests.Tracks.Commands.AddTrack;
using RtfmAPI.Application.Requests.Tracks.Queries.GetTrackById;
using RtfmAPI.Application.Requests.Tracks.Queries.GetTracks;

namespace RtfmAPI.Presentation.Controllers;

/// <summary>
/// Контроллер музыкальных треков
/// </summary>
public class TracksController : ApiControllerBase
{
    /// <summary>
    /// Контроллер музыкальных треков
    /// </summary>
    /// <param name="mediator">Медиатор</param>
    public TracksController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Получение музыкальных треков
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Музыкальные треки</returns>
    [HttpGet]
    public Task<List<Track>> GetTracksAsync(CancellationToken cancellationToken =default)
    {
        var query = new GetTracksQuery();

        return Mediator.Send(query, cancellationToken);
    }

    /// <summary>
    /// Получение музыкального трека по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Музыкальный трек</returns>
    [HttpGet("{id}")]
    public Task<Track?> GetTrackByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var query = new GetTrackByIdQuery
        {
            Id = id
        };

        return Mediator.Send(query, cancellationToken);
    }

    /// <summary>
    /// Добавление музыкального трека
    /// </summary>
    /// <param name="name">Название музыкального трека</param>
    /// <param name="releaseDate">Дата выпуска</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Добавленный музыкальный трек</returns>
    [HttpPost]
    public Task<Track> AddTrackAsync([FromQuery] string name, [FromQuery] DateTime releaseDate, CancellationToken cancellationToken = default)
    {
        var command = new AddTrackCommand
        {
            Name = name,
            ReleaseDate = releaseDate
        };

        return Mediator.Send(command, cancellationToken);
    }
}