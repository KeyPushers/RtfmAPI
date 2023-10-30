using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RftmAPI.Domain.Models.Bands;
using RftmAPI.Domain.Utils;
using RtfmAPI.Application.Common.Interfaces.Persistence;

namespace RtfmAPI.Application.Requests.Bands.Commands.AddBand;

/// <summary>
/// Обработчик команды добавления музыкальной группы.
/// </summary>
public class AddBandCommandHandler : IRequestHandler<AddBandCommand, Band>
{
    private readonly IBandsRepository _bandsRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Обработчик команды добавления музыкальной группы.
    /// </summary>
    /// <param name="bandsRepository">Репозиторий групп.</param>
    /// <param name="unitOfWork">Единица работы.</param>
    public AddBandCommandHandler(IBandsRepository bandsRepository, IUnitOfWork unitOfWork)
    {
        _bandsRepository = bandsRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Обработка команды добавления музыкальной группы.
    /// </summary>
    /// <param name="request">Команда добавления музыкальной группы.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Музыкальная группа.</returns>
    public async Task<Band> Handle(AddBandCommand request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}