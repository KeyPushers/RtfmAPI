using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RftmAPI.Domain.Models.Bands;
using RftmAPI.Domain.Models.Bands.Repository;
using RftmAPI.Domain.Models.Bands.ValueObjects;

namespace RtfmAPI.Application.Requests.Bands.Queries.GetBandById;

/// <summary>
/// Обработчик запроса музыкальной группы по идентификатору.
/// </summary>
public class GetBandByIdQueryHandler : IRequestHandler<GetBandByIdQuery, Band?>
{
    private readonly IBandsRepository _bandsRepository;

    /// <summary>
    /// Обработчик запроса музыкальной группы по идентификатору.
    /// </summary>
    /// <param name="bandsRepository">Репозиторий музыкальных групп.</param>
    public GetBandByIdQueryHandler(IBandsRepository bandsRepository)
    {
        _bandsRepository = bandsRepository;
    }
    
    /// <summary>
    /// Обработка запроса музыкальной группы по идентификатору.
    /// </summary>
    /// <param name="request">Запрос музыкальной группы по идентификатору.</param>
    /// <param name="cancellationToken">Токен отменеы</param>
    /// <returns>Музыкальная группа.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<Band?> Handle(GetBandByIdQuery request, CancellationToken cancellationToken = default)
    {
        return _bandsRepository.GetBandByIdAsync(BandId.Create(request.Id));
    }
}