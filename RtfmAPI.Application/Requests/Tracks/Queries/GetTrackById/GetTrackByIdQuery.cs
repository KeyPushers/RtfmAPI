using System;
using MediatR;
using RftmAPI.Domain.Primitives;
using RtfmAPI.Application.Requests.Tracks.Queries.GetTrackById.Dtos;

namespace RtfmAPI.Application.Requests.Tracks.Queries.GetTrackById;

/// <summary>
/// Запрос музыкального трека по идентификатору
/// </summary>
public class GetTrackByIdQuery : IRequest<Result<GetTrackByIdResponse?>>
{
    /// <summary>
    /// Идентификатор музыкального трека
    /// </summary>
    public Guid Id { get; init; }
}