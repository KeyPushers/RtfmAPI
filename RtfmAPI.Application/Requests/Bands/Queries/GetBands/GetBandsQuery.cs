using MediatR;
using RftmAPI.Domain.Primitives;
using RtfmAPI.Application.Requests.Bands.Queries.GetBands.Dtos;

namespace RtfmAPI.Application.Requests.Bands.Queries.GetBands;

/// <summary>
/// Запрос музыкальных групп.
/// </summary>
public class GetBandsQuery : IRequest<Result<BandItems>>
{
    
}