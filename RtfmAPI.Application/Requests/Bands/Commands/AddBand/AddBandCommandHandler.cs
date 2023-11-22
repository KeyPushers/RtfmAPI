using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RftmAPI.Domain.Models.Bands;
using RftmAPI.Domain.Models.Bands.ValueObjects;
using RftmAPI.Domain.Primitives;
using RtfmAPI.Application.Common.Interfaces.Persistence;
using RtfmAPI.Application.Requests.Bands.Commands.AddBand.Dtos;

namespace RtfmAPI.Application.Requests.Bands.Commands.AddBand;

/// <summary>
/// Обработчик команды добавления музыкальной группы.
/// </summary>
public class AddBandCommandHandler : IRequestHandler<AddBandCommand, Result<AddedBand>>
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
    public async Task<Result<AddedBand>> Handle(AddBandCommand request, CancellationToken cancellationToken = default)
    {
        var bandNameResult = BandName.Create(request.Name);
        if (bandNameResult.IsFailed)
        {
            return bandNameResult.Error;
        }

        var bandResult = Band.Create(bandNameResult.Value);
        if (bandResult.IsFailed)
        {
            return bandResult.Error;
        }

        await _bandsRepository.AddAsync(bandResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var band = bandResult.Value;
        
        return new AddedBand
        {
            Id = band.Id.Value,
            Name = band.Name.Value
        };
    }
}