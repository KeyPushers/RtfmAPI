using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RftmAPI.Domain.Exceptions.BandExceptions;
using RftmAPI.Domain.Models.Bands.ValueObjects;
using RftmAPI.Domain.Primitives;
using RtfmAPI.Application.Common.Interfaces.Persistence;
using RtfmAPI.Application.Requests.Bands.Queries.GetBandInfo.Dtos;

namespace RtfmAPI.Application.Requests.Bands.Queries.GetBandInfo;

/// <summary>
/// Обработчик запроса музыкальной группы.
/// </summary>
public class GetBandInfoQueryHandler : IRequestHandler<GetBandInfoQuery, Result<BandInfo>>
{
    private readonly IBandsRepository _bandsRepository;

    /// <summary>
    /// Обработчик запроса музыкальной группы.
    /// </summary>
    /// <param name="bandsRepository">Репозиторий музыкальных групп.</param>
    public GetBandInfoQueryHandler(IBandsRepository bandsRepository)
    {
        _bandsRepository = bandsRepository;
    }
    
    /// <summary>
    /// Обработка запроса музыкальной группы.
    /// </summary>
    /// <param name="request">Запрос музыкальной группы по идентификатору.</param>
    /// <param name="cancellationToken">Токен отменеы</param>
    /// <returns>Музыкальная группа.</returns>
    public async Task<Result<BandInfo>> Handle(GetBandInfoQuery request, CancellationToken cancellationToken = default)
    {
        var bandId = BandId.Create(request.Id);
        var band = await _bandsRepository.GetBandByIdAsync(bandId);
        if (band is null)
        {
            return BandExceptions.NotFound(bandId);
        }
        
        return new BandInfo
        {
            Name = band.Name.Value
        };
    }
}