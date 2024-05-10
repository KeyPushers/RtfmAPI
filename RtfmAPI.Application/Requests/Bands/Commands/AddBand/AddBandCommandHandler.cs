using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using MediatR;
using RtfmAPI.Application.Fabrics.Bands;
using RtfmAPI.Application.Interfaces.Persistence.Commands;
using RtfmAPI.Application.Requests.Bands.Commands.AddBand.Dtos;

namespace RtfmAPI.Application.Requests.Bands.Commands.AddBand;

/// <summary>
/// Обработчик команды добавления музыкальной группы.
/// </summary>
public class AddBandCommandHandler : IRequestHandler<AddBandCommand, Result<AddedBand>>
{
    private readonly IBandsCommandsRepository _repository;

    /// <summary>
    /// Обработчик команды добавления музыкальной группы.
    /// </summary>
    /// <param name="repository">Репозиторий.</param>
    public AddBandCommandHandler(IBandsCommandsRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Обработка команды добавления музыкальной группы.
    /// </summary>
    /// <param name="request">Команда добавления музыкальной группы.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Музыкальная группа.</returns>
    public async Task<Result<AddedBand>> Handle(AddBandCommand request, CancellationToken cancellationToken = default)
    {
        var bandsFactory = new BandsFactory(new()
        {
            Name = request.Name
        });

        var createBandResult = bandsFactory.Create();
        if (createBandResult.IsFailed)
        {
            return createBandResult.ToResult();
        }

        var band = createBandResult.ValueOrDefault;
        await _repository.CommitChangesAsync(band, cancellationToken);

        return new AddedBand
        {
            Id = band.Id.Value,
            Name = band.Name.Value
        };
    }
}