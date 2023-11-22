using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RftmAPI.Domain.Models.Bands;
using RftmAPI.Domain.Primitives;
using RtfmAPI.Application.Common.Interfaces.Persistence;
using RtfmAPI.Application.Requests.Bands.Queries.GetBands.Dtos;

namespace RtfmAPI.Application.Requests.Bands.Queries.GetBands;

/// <summary>
/// Обработчик запроса музыкальных групп.
/// </summary>
public class GetBandsQueryHandler : IRequestHandler<GetBandsQuery, Result<BandItems>>
{
    private readonly IBandsRepository _bandsRepository;
    
    /// <summary>
    /// Обработчик запроса музыкальных групп.
    /// </summary>
    /// <param name="bandsRepository">Репозиторий групп.</param>
    public GetBandsQueryHandler(IBandsRepository bandsRepository)
    {
        _bandsRepository = bandsRepository;
    }

    /// <summary>
    /// Обработка запроса музыкальных групп.
    /// </summary>
    /// <param name="request">Запрос музыкальных групп.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Музыкальные группы.</returns>
    public async Task<Result<BandItems>> Handle(GetBandsQuery request, CancellationToken cancellationToken = default)
    {
        var result = await _bandsRepository.GetBandsAsync();
        
        return GetBandItems(result);
    }

    /// <summary>
    /// Объект переноса данных музыкальных групп.
    /// </summary>
    /// <param name="bands">Музыкальная группы.</param>
    /// <returns>Объект переноса данных музыкальных групп.</returns>
    private static BandItems GetBandItems(IEnumerable<Band> bands)
    {
        return new BandItems
        {
            Bands = bands.Select(GetBandItem).ToList()
        };
    }

    /// <summary>
    /// Получение объекта переноса данных музыкальной группы.
    /// </summary>
    /// <param name="band">Музыкальная группа.</param>
    /// <returns>Объект переноса данных музыкальной группы.</returns>
    private static BandItem GetBandItem(Band band)
    {
        return new BandItem
        {
            Id = band.Id.Value,
            Name = band.Name.Value
        };
    } 
}