using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RftmAPI.Domain.Exceptions.TrackExceptions;
using RftmAPI.Domain.Models.TrackFiles;
using RftmAPI.Domain.Models.TrackFiles.Repository;
using RftmAPI.Domain.Models.TrackFiles.ValueObjects;
using RftmAPI.Domain.Models.Tracks;
using RftmAPI.Domain.Models.Tracks.Repository;
using RftmAPI.Domain.Models.Tracks.ValueObjects;
using RftmAPI.Domain.Primitives;
using RftmAPI.Domain.Utils;

namespace RtfmAPI.Application.Requests.Tracks.Commands.AddTrack;

/// <summary>
/// Обработчик команды добавления музыкального трека
/// </summary>
public class AddTrackCommandHandler : IRequestHandler<AddTrackCommand, Result<Track>>
{
    private readonly ITracksRepository _tracksRepository;
    private readonly ITrackFilesRepository _trackFilesRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Обработчик команды добавления музыкального трека.
    /// </summary>
    /// <param name="tracksRepository">Репозиторий музыкальных треков.</param>
    /// <param name="trackFilesRepository">Репозиторий файлов музыкальных треков.</param>
    /// <param name="unitOfWork">Единица работы.</param>
    public AddTrackCommandHandler(ITracksRepository tracksRepository, ITrackFilesRepository trackFilesRepository,
        IUnitOfWork unitOfWork)
    {
        _tracksRepository = tracksRepository;
        _trackFilesRepository = trackFilesRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Обработка команды добавления музыкального трека.
    /// </summary>
    /// <param name="request">Команда добавления музыкального трека.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Музыкальный трек.</returns>
    public async Task<Result<Track>> Handle(AddTrackCommand request, CancellationToken cancellationToken = default)
    {
        var trackNameResult = TrackName.Create(request.Name ?? string.Empty);
        if (trackNameResult.IsFailed)
        {
            return trackNameResult.Error;
        }

        var trackReleaseDateResult = TrackReleaseDate.Create(request.ReleaseDate);
        if (trackReleaseDateResult.IsFailed)
        {
            return trackReleaseDateResult.Error;
        }

        if (request.TrackFile is null)
        {
            return TrackExceptions.TrackFileDataExceptions.IsEmpty;
        }

        var trackFileResult = CreateTrackFile(request.TrackFile);
        if (trackFileResult.IsFailed)
        {
            return trackFileResult.Error;
        }
        
        var trackResult = Track.Create(trackNameResult.Value, trackReleaseDateResult.Value, trackFileResult.Value);
        if (trackResult.IsFailed)
        {
            return trackResult.Error;
        }

        await _trackFilesRepository.AddAsync(trackFileResult.Value);
        await _tracksRepository.AddAsync(trackResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return trackResult.Value;
    }

    /// <summary>
    /// Создание представления данных файла музыкального трека (<see cref="TrackFile"/>).
    /// </summary>
    /// <param name="trackFile">Объект переноса данных файла музыкального трека.</param>
    /// <returns>Представление данных файла музыкального трека.</returns>
    private static Result<TrackFile> CreateTrackFile(Dtos.TrackFile trackFile)
    {
        var trackFileNameResult = TrackFileName.Create(trackFile.FileName ?? string.Empty);
        if (trackFileNameResult.IsFailed)
        {
            return trackFileNameResult.Error;
        }

        var trackFileExtensionResult = TrackFileExtension.Create(trackFile.Extension ?? string.Empty);
        if (trackFileExtensionResult.IsFailed)
        {
            return trackFileExtensionResult.Error;
        }

        var trackFileMimeTypeResult = TrackFileMimeType.Create(trackFile.MimeType ?? string.Empty);
        if (trackFileMimeTypeResult.IsFailed)
        {
            return trackFileMimeTypeResult.Error;
        }

        var trackFileDataResult = TrackFileData.Create(trackFile.File?.ToArray() ?? Array.Empty<byte>());
        if (trackFileDataResult.IsFailed)
        {
            return trackFileDataResult.Error;
        }

        return TrackFile.Create(trackFileNameResult.Value, trackFileDataResult.Value, trackFileExtensionResult.Value,
            trackFileMimeTypeResult.Value);
    }
}