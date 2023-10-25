using System;
using MediatR;
using RftmAPI.Domain.Models.Albums;

namespace RtfmAPI.Application.Requests.Albums.Commands.AddAlbum;

/// <summary>
/// Команда добавления музыкального альбома
/// </summary>
public class AddAlbumCommand : IRequest<Album>
{
    /// <summary>
    /// Название альбома
    /// </summary>
    public string? Name { get; init; }
    
    /// <summary>
    /// Дата релиза
    /// </summary>
    public DateTime ReleaseDate { get; init; }
}