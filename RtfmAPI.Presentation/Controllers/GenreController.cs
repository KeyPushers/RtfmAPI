using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RtfmAPI.Application.Requests.Bands.Commands.AddBand;
using RtfmAPI.Application.Requests.Genres.Commands.AddGenre;
using RtfmAPI.Application.Requests.Genres.Commands.AddGenre.Dtos;

namespace RtfmAPI.Presentation.Controllers;

/// <summary>
/// Контроллер музыкальных жанров.
/// </summary>
public class GenreController : ApiControllerBase
{
    /// <summary>
    /// Создание контроллера музыкальных жанров.
    /// </summary>
    /// <param name="mediator">Медиатор.</param>
    public GenreController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Добавление музыкального жанра.
    /// </summary>
    /// <param name="request">Объект переноса данных добавляемого музыкального жанра.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Объект переноса данных добавленного музыкального жанра.</returns>
    [HttpPost]
    public async Task<ActionResult<AddedGenre>> AddGenreAsync([FromBody] AddingGenre request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(nameof(request.Name));
        }

        var genreCommand = new AddGenreCommand(request.Name);
        var commandResult = await Mediator.Send(genreCommand, cancellationToken);
        if (commandResult.IsFailed)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, commandResult.Error);
        }

        // TODO: Изменить возвращаемый тип по завершению метода по получению музыкального жанра.
        return Ok(commandResult.Value);
    }
}