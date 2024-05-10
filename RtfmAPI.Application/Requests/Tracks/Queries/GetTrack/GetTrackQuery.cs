using System;
using FluentResults;
using MediatR;
using RtfmAPI.Application.Requests.Tracks.Queries.GetTrack.Dtos;

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