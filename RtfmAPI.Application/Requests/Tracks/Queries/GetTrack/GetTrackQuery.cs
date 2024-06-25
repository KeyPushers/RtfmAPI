using System;
using MediatR;
using RtfmAPI.Application.Requests.Tracks.Queries.GetTrack.Dtos;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Application.Requests.Tracks.Queries.GetTrack;

/// <summary>
/// Запрос информации о музыкальном треке.
/// </summary>
public class GetTrackQuery : IRequest<Result<TrackInfo>>
{
    /// <summary>
    /// Идентификатор музыкального трека.
    /// </summary>
    public Guid Id { get; init; }
}