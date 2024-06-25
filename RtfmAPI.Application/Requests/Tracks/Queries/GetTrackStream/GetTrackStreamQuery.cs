using System;
using MediatR;
using RtfmAPI.Application.Requests.Tracks.Queries.GetTrackStream.Dtos;
using RtfmAPI.Domain.Primitives;

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