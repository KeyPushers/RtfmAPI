using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RftmAPI.Domain.Exceptions.BandExceptions;
using RftmAPI.Domain.Models.Bands.ValueObjects;
using RftmAPI.Domain.Primitives;
using RtfmAPI.Application.Common.Interfaces.Persistence;

namespace RtfmAPI.Application.Requests.Bands.Commands.DeleteBandById;

/// <summary>
/// Обработчик команды удаления музыкальной группы по идентификатору.
/// </summary>
public class DeleteBandByIdCommandHandler : IRequestHandler<DeleteBandByIdCommand, Result<Unit>>
{
    private readonly IBandsRepository _bandsRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Создание обработчика команды удаления музыкальной группы по идентификатору.
    /// </summary>
    /// <param name="bandsRepository">Репозиторий музыкальной группы.</param>
    /// <param name="unitOfWork">Единица работы.</param>
    public DeleteBandByIdCommandHandler(IBandsRepository bandsRepository, IUnitOfWork unitOfWork)
    {
        _bandsRepository = bandsRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Обработка команды удаления музыкальной группы по идентификатору.
    /// </summary>
    /// <param name="request">Команда удаления музыкальной группы по идентификатору.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task<Result<Unit>> Handle(DeleteBandByIdCommand request, CancellationToken cancellationToken = default)
    {
        var bandId = BandId.Create(request.Id);
        var band = await _bandsRepository.GetBandByIdAsync(bandId);
        if (band is null)
        {
            return BandExceptions.NotFound(bandId);
        }

        var deleteBandResult = await band.DeleteAsync(_bandsRepository.DeleteAsync);
        if (deleteBandResult.IsFailed)
        {
            return deleteBandResult.Error;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}