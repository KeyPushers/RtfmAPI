using System;
using FluentResults;
using MediatR;
using RtfmAPI.Application.Requests.Albums.Commands.AddAlbum.Dtos;

namespace RtfmAPI.Application.Requests.Albums.Commands.AddAlbum;

/// <summary>
/// Команда добавления музыкального альбома.
/// </summary>
public class AddAlbumCommand : IRequest<Result<AddedAlbum>>
{
    /// <summary>
    /// Название альбома.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Дата релиза.
    /// </summary>
    public DateTime ReleaseDate { get; init; }
}