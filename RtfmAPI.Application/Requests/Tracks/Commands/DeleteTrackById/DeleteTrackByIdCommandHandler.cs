using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RftmAPI.Domain.Exceptions.TrackExceptions;
using RftmAPI.Domain.Exceptions.TrackFileExceptions;
using RftmAPI.Domain.Models.Tracks.ValueObjects;
using RftmAPI.Domain.Primitives;
using RtfmAPI.Application.Common.Interfaces.Persistence;

namespace RtfmAPI.Application.Requests.Tracks.Commands.DeleteTrackById;

/// <summary>
/// Обработчик команды удаления музыкального трека по идентификатору.
/// </summary>
public class DeleteTrackByIdCommandHandler : IRequestHandler<DeleteTrackByIdCommand, Result<Unit>>
{
    private readonly ITracksRepository _tracksRepository;
    private readonly ITrackFilesRepository _trackFilesRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Создание обработчика команды удаления музыкального трека по идентификатору.
    /// </summary>
    /// <param name="tracksRepository">Репозиторий музыкальных треков.</param>
    /// <param name="trackFilesRepository">Репозиторий файлов музыкальных треков.</param>
    /// <param name="unitOfWork">Единица работы.</param>
    public DeleteTrackByIdCommandHandler(ITracksRepository tracksRepository, ITrackFilesRepository trackFilesRepository,
        IUnitOfWork unitOfWork)
    {
        _tracksRepository = tracksRepository;
        _trackFilesRepository = trackFilesRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Обработка команды удаления музыкального трека по идентификатору.
    /// </summary>
    /// <param name="request">Команда удаления музыкального трека по идентификатору.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task<Result<Unit>> Handle(DeleteTrackByIdCommand request, CancellationToken cancellationToken)
    {
        var trackId = TrackId.Create(request.Id);
        var track = await _tracksRepository.GetTrackByIdAsync(trackId);
        if (track is null)
        {
            return TrackExceptions.NotFound(trackId);
        }

        var trackFileId = track.TrackFileId;
        var trackFile = await _trackFilesRepository.GetTrackFileByIdAsync(trackFileId);
        if (trackFile is null)
        {
            return TrackFileExceptions.NotFound(trackFileId);
        }

        var deleteTrackResult = await track.DeleteAsync(async t =>
        {
            var deleteTrackFileResult = await trackFile.DeleteAsync(_trackFilesRepository.DeleteAsync);
            return !deleteTrackFileResult.IsFailed && await _tracksRepository.DeleteAsync(t);
        });
        if (deleteTrackResult.IsFailed)
        {
            return deleteTrackResult.Error;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}