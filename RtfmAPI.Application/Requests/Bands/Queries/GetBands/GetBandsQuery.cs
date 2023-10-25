using System.Collections.Generic;
using MediatR;
using RftmAPI.Domain.Models.Bands;

namespace RtfmAPI.Application.Requests.Bands.Queries.GetBands;

/// <summary>
/// Запрос музыкальных групп.
/// </summary>
public class GetBandsQuery : IRequest<List<Band>>
{
    
}