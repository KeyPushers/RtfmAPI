using System;
using MediatR;
using RftmAPI.Domain.Primitives;

namespace RtfmAPI.Application.Requests.Tracks.Commands.AddAlbumToTrack;

/// <summary>
/// Команда добавления музыкального альбома к музыкальному треку.
/// </summary>
public class AddAlbumToTrackCommand : IRequest<Result<Unit>>
{
    /// <summary>
    /// Идентификатор музыкального трека.
    /// </summary>
    public Guid TrackId { get; init; }

    /// <summary>
    /// Идентификатор музыкального альбома.
    /// </summary>
    public Guid AlbumId { get; init; }
}