using System;
using MediatR;
using RftmAPI.Domain.Aggregates.Tracks;

namespace RtfmAPI.Application.Requests.Tracks.Queries.GetTrackById;

/// <summary>
/// Запрос музыкального трека по идентификатору
/// </summary>
public class GetTrackByIdQuery : IRequest<Track?>
{
    /// <summary>
    /// Идентификатор музыкального трека
    /// </summary>
    public Guid Id { get; init; }
}