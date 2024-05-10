using System;
using FluentResults;
using MediatR;
using RtfmAPI.Application.Requests.Tracks.Commands.AddTrack.Dtos;

namespace RtfmAPI.Application.Requests.Tracks.Commands.AddTrack;

/// <summary>
/// Команда добавления музыкального трека
/// </summary>
public class AddTrackCommand : IRequest<Result<AddedTrack>>
{
    /// <summary>
    /// Название трека
    /// </summary>
    public string? Name { get; init; }
    
    /// <summary>
    /// Дата релиза
    /// </summary>
    public DateTime ReleaseDate { get; init; }

    /// <summary>
    /// Файл музыкального трека.
    /// </summary>
    public AddingTrack? TrackFile { get; init; }
}