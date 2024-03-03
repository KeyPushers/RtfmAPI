using System;
using MediatR;
using RtfmAPI.Application.Requests.Bands.Queries.GetBandInfo.Dtos;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Application.Requests.Bands.Queries.GetBandInfo;

/// <summary>
/// Запрос музыкальной группы.
/// </summary>
public class GetBandInfoQuery : IRequest<Result<BandInfo>>
{
    /// <summary>
    /// Идентификатор музыкальной группы.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Создание запроса музыкальной группы.
    /// </summary>
    /// <param name="id">Идентификатор музыкальной группы.</param>
    public GetBandInfoQuery(Guid id)
    {
        Id = id;
    }
}