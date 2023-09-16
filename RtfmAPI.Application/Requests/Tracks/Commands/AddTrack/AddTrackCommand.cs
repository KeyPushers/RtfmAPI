using System;
using MediatR;
using RftmAPI.Domain.Aggregates.Tracks;

namespace RtfmAPI.Application.Requests.Tracks.Commands.AddTrack;

/// <summary>
/// Команда добавления музыкального трека
/// </summary>
public class AddTrackCommand : IRequest<Track>
{
    /// <summary>
    /// Название трека
    /// </summary>
    public string? Name { get; init; }
    
    /// <summary>
    /// Дата релиза
    /// </summary>
    public DateTime ReleaseDate { get; init; }
}