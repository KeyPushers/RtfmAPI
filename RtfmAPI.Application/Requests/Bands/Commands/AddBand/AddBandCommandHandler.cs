using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RtfmAPI.Application.Interfaces.Persistence.Commands;
using RtfmAPI.Application.Requests.Bands.Commands.AddBand.Dtos;
using RtfmAPI.Domain.Models.Bands;
using RtfmAPI.Domain.Primitives;

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
        var bandsFabric = new BandsFabric(request.Name, Enumerable.Empty<Guid>(), Enumerable.Empty<Guid>());
        var createBandResult = bandsFabric.Create();
        if (createBandResult.IsFailed)
        {
            return createBandResult.Error;
        }

        var band = createBandResult.Value;
        await _repository.CommitChangesAsync(band, cancellationToken);

        return new AddedBand
        {
            Id = band.Id.Value,
            Name = band.Name.Value
        };
    }
}