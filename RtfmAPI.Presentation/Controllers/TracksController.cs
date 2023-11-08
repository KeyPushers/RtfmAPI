using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RtfmAPI.Application.Requests.Tracks.Commands.AddAlbumToTrack;
using RtfmAPI.Application.Requests.Tracks.Commands.AddTrack;
using RtfmAPI.Application.Requests.Tracks.Commands.AddTrack.Dtos;
using RtfmAPI.Application.Requests.Tracks.Queries.GetTrack;
using RtfmAPI.Application.Requests.Tracks.Queries.GetTrack.Dtos;
using RtfmAPI.Application.Requests.Tracks.Queries.GetTrackById;
using RtfmAPI.Application.Requests.Tracks.Queries.GetTracks;
using RtfmAPI.Application.Requests.Tracks.Queries.GetTracks.Dtos;
using RtfmAPI.Presentation.Dtos.Tracks;

namespace RtfmAPI.Presentation.Controllers;

/// <summary>
/// Контроллер музыкальных треков.
/// </summary>
public class TracksController : ApiControllerBase
{
    /// <summary>
    /// Создание контроллера музыкальных треков.
    /// </summary>
    /// <param name="mediator">Медиатор.</param>
    public TracksController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Получение музыкальных треков.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Музыкальные треки.</returns>
    [HttpGet]
    public async Task<ActionResult<TrackItems>> GetTracksAsync(CancellationToken cancellationToken = default)
    {
        var query = new GetTracksQuery();
        var queryResult = await Mediator.Send(query, cancellationToken);
        if (queryResult.IsFailed)
        {
            return BadRequest(queryResult.Error.Message);
        }

        return Ok(queryResult.Value);
    }

    /// <summary>
    /// Получение потока музыкального трека по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Музыкальный трек.</returns>
    [HttpGet("{id:guid}/stream", Name = nameof(GetTrackStreamByIdAsync))]
    public async Task<IActionResult> GetTrackStreamByIdAsync([FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        // TODO: Переработать.
        var query = new GetTrackByIdQuery
        {
            Id = id
        };

        var result = await Mediator.Send(query, cancellationToken);
        if (result.IsFailed)
        {
            return BadRequest(result.Error);
        }

        if (result.Value?.Stream is null)
        {
            return BadRequest(result.Error);
        }

        return new FileStreamResult(result.Value.Stream, result.Value?.MediaType ?? string.Empty)
        {
            EnableRangeProcessing = true
        };
    }

    /// <summary>
    /// Получение инофрмации о музыкальном треке по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Музыкальный трек.</returns>
    [HttpGet("{id:guid}", Name = nameof(GetTrackInfoAsync))]
    public async Task<ActionResult<TrackInfo>> GetTrackInfoAsync([FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetTrackQuery
        {
            Id = id
        };

        var queryResult = await Mediator.Send(query, cancellationToken);
        if (queryResult.IsFailed)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, queryResult.Error);
        }
        
        return Ok(queryResult.Value);
    }
    
    /// <summary>
    /// Добавление музыкального трека.
    /// </summary>
    /// <param name="request">Объект переноса команды добавления музыкального трека.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Объект переноса данных добавленного музыкального трека.</returns>
    [HttpPost]
    public async Task<ActionResult<AddedTrack>> AddTrackAsync([FromForm] AddTrack request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var memoryStream = new MemoryStream();
            if (request.File is null)
            {
                return BadRequest(nameof(request.File));
            }

            await request.File.CopyToAsync(memoryStream, cancellationToken);

            var command = new AddTrackCommand
            {
                Name = request.Name,
                ReleaseDate = request.ReleaseDate,
                TrackFile = new AddingTrack(memoryStream)
                {
                    FileName = request.File.FileName,
                    Extension = Path.GetExtension(request.File.FileName),
                    MimeType = request.File.ContentType
                }
            };

            var commandResult = await Mediator.Send(command, cancellationToken);
            if (commandResult.IsFailed)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, commandResult.Error);
            }

            return CreatedAtRoute(nameof(GetTrackInfoAsync), new {commandResult.Value.Id}, commandResult.Value);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex);
        }
    }

    /// <summary>
    /// Добавленеи музыкального альбома к музыкальному треку.
    /// </summary>
    /// <param name="trackId">Идентификатор музыкального трека.</param>
    /// <param name="albumId">Идентификатор музыкального альбома.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [HttpPost("[action]")]
    public async Task<IActionResult> AddAlbumToTrackAsync([FromQuery] Guid trackId, [FromQuery] Guid albumId,
        CancellationToken cancellationToken = default)
    {
        var command = new AddAlbumToTrackCommand
        {
            TrackId = trackId,
            AlbumId = albumId
        };

        var commandResult = await Mediator.Send(command, cancellationToken);
        if (commandResult.IsFailed)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, commandResult.Error);
        }

        return Ok(commandResult.Value);
    }
}