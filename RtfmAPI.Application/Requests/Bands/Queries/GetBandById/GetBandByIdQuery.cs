using System;
using MediatR;
using RftmAPI.Domain.Models.Bands;

namespace RtfmAPI.Application.Requests.Bands.Queries.GetBandById;

/// <summary>
/// Запрос музыкальной группы по идентификатору.
/// </summary>
public class GetBandByIdQuery : IRequest<Band?>
{
    /// <summary>
    /// Идентификатор музыкальной группы.
    /// </summary>
    public Guid Id { get; init; }
}