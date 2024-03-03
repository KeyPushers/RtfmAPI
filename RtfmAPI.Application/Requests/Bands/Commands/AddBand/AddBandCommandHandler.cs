using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RtfmAPI.Application.Fabrics;
using RtfmAPI.Application.Interfaces.Persistence.Commands;
using RtfmAPI.Application.Requests.Bands.Commands.AddBand.Dtos;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Application.Requests.Bands.Commands.AddBand;

/// <summary>
/// Обработчик команды добавления музыкальной группы.
/// </summary>
public class AddBandCommandHandler : IRequestHandler<AddBandCommand, Result<AddedBand>>
{
    private readonly IBandsCommandsRepository _repository;
    private readonly BandsFabric _bandsFabric;

    /// <summary>
    /// Обработчик команды добавления музыкальной группы.
    /// </summary>
    /// <param name="repository">Репозиторий.</param>
    /// <param name="bandsFabric">Фабрика музыкальных групп.</param>
    public AddBandCommandHandler(IBandsCommandsRepository repository, BandsFabric bandsFabric)
    {
        _repository = repository;
        _bandsFabric = bandsFabric;
    }

    /// <summary>
    /// Обработка команды добавления музыкальной группы.
    /// </summary>
    /// <param name="request">Команда добавления музыкальной группы.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Музыкальная группа.</returns>
    public async Task<Result<AddedBand>> Handle(AddBandCommand request, CancellationToken cancellationToken = default)
    {
        var getBandResult = _bandsFabric.CreateBand(request.Name);
        if (getBandResult.IsFailed)
        {
            return getBandResult.Error;
        }
        
        var band = getBandResult.Value;
        await _repository.CommitChangesAsync(band);
        
        return new AddedBand
        {
            Id = band.Id.Value,
            Name = band.Name.Value
        };
    }
}