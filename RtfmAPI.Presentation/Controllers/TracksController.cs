﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RftmAPI.Domain.Models.Tracks;
using RtfmAPI.Application.Requests.Tracks.Commands.AddTrack;
using RtfmAPI.Application.Requests.Tracks.Commands.AddTrack.Dtos;
using RtfmAPI.Application.Requests.Tracks.Queries.GetTrackById;
using RtfmAPI.Application.Requests.Tracks.Queries.GetTracks;
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
    public Task<List<Track>> GetTracksAsync(CancellationToken cancellationToken = default)
    {
        var query = new GetTracksQuery();

        return Mediator.Send(query, cancellationToken);
    }

    /// <summary>
    /// Получение музыкального трека по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Музыкальный трек.</returns>
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
    /// Добавление музыкального трека.
    /// </summary>
    /// <param name="request">Объект переноса команды добавления музыкального трека.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Добавленный музыкальный трек.</returns>
    [HttpPost]
    public async Task<Track> AddTrackAsync([FromForm] AddTrack request, CancellationToken cancellationToken = default)
    {
        using var memoryStream = new MemoryStream();
        if (request.File is null)
        {
            throw new Exception($"{nameof(request.File)}");
        }

        await request.File.CopyToAsync(memoryStream, cancellationToken);

        var command = new AddTrackCommand
        {
            Name = request.Name,
            ReleaseDate = request.ReleaseDate,
            TrackFile = new TrackFile
            {
                File = memoryStream,
                FileName = request.File.FileName,
                Extension = Path.GetExtension(request.File.FileName),
                MimeType = request.File.ContentType
            }
        };

        return await Mediator.Send(command, cancellationToken);
    }
}