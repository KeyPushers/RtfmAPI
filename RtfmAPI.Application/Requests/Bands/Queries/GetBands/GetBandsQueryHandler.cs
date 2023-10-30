using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RftmAPI.Domain.Models.Bands;
using RtfmAPI.Application.Common.Interfaces.Persistence;

namespace RtfmAPI.Application.Requests.Bands.Queries.GetBands;

/// <summary>
/// Обработчик запроса музыкальных групп.
/// </summary>
public class GetBandsQueryHandler : IRequestHandler<GetBandsQuery, List<Band>>
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
    public async Task<List<Band>> Handle(GetBandsQuery request, CancellationToken cancellationToken = default)
    {
        var result = await _bandsRepository.GetBandsAsync().ConfigureAwait(false);
        return result;
    }
}