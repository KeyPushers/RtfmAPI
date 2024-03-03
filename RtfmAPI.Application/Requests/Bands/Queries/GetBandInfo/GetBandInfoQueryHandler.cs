using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RtfmAPI.Application.Interfaces.Persistence.Queries;
using RtfmAPI.Application.Requests.Bands.Queries.GetBandInfo.Dtos;
using RtfmAPI.Domain.Models.Bands;
using RtfmAPI.Domain.Models.Bands.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Application.Requests.Bands.Queries.GetBandInfo;

/// <summary>
/// Обработчик запроса музыкальной группы.
/// </summary>
public class GetBandInfoQueryHandler : IRequestHandler<GetBandInfoQuery, Result<BandInfo>>
{
    private readonly IBandsQueriesRepository _repository;

    /// <summary>
    /// Обработчик запроса музыкальной группы.
    /// </summary>
    /// <param name="repository">Репозитория запросов доменной модели <see cref="Band"/>.</param>
    public GetBandInfoQueryHandler(IBandsQueriesRepository repository)
    {
        _repository = repository;
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
        var getBandResult = await _repository.GetBandByIdAsync(bandId);
        if (getBandResult.IsFailed)
        {
            return getBandResult.Error;
        }

        var band = getBandResult.Value;
        
        return new BandInfo
        {
            Name = band.Name.Value
        };
    }
}