using System;
using FluentResults;
using MediatR;
using RtfmAPI.Application.Requests.Tracks.Queries.GetTrackStream.Dtos;

namespace RtfmAPI.Application.Requests.Tracks.Queries.GetTrackStream;

/// <summary>
/// Запрос потока музыкального трека.
/// </summary>
public class GetTrackStreamQuery : IRequest<Result<TrackStream>>
{
    /// <summary>
    /// Идентификатор музыкального трека
    /// </summary>
    public Guid Id { get; init; }
}