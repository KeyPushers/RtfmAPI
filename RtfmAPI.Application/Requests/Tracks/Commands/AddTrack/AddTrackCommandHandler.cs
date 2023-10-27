using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;
using RftmAPI.Domain.Models.Albums.Repository;
using RftmAPI.Domain.Models.Tracks;
using RftmAPI.Domain.Models.Tracks.Entities;
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
    private readonly IAlbumsRepository _albumsRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Обработчик команды добавления музыкального трека.
    /// </summary>
    /// <param name="tracksRepository">Репозиторий музыкальных треков.</param>
    /// <param name="albumsRepository">Репозиторий альбомов.</param>
    /// <param name="unitOfWork">Единица работы.</param>
    public AddTrackCommandHandler(ITracksRepository tracksRepository, IAlbumsRepository albumsRepository,
        IUnitOfWork unitOfWork)
    {
        _tracksRepository = tracksRepository;
        _albumsRepository = albumsRepository;
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
            throw new Exception(trackNameResult.Errors.FirstOrDefault()?.Message);
        }

        var trackReleaseDateResult = TrackReleaseDate.Create(request.ReleaseDate);
        if (trackReleaseDateResult.IsFailed)
        {
            throw new Exception(trackReleaseDateResult.Errors.FirstOrDefault()?.Message);
        }

        if (request.TrackFile is null)
        {
            throw new Exception(nameof(request.TrackFile));
        }

        var trackFileResult = CreateTrackFile(request.TrackFile);
        if (trackFileResult is null || trackFileResult.IsFailed)
        {
            throw new Exception(nameof(request.TrackFile));
        }

        var trackResult = Track.Create(trackNameResult.Value, trackReleaseDateResult.Value, trackFileResult.Value);
        if (trackResult.IsFailed)
        {
            throw new Exception(nameof(request.TrackFile));
        }

        await _tracksRepository.AddAsync(trackResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return trackResult.Value;
    }

    /// <summary>
    /// Создание представления данных файла музыкального трека (<see cref="TrackFile"/>).
    /// </summary>
    /// <param name="trackFile">Объект переноса данных файла музыкального трека.</param>
    /// <returns>Представление данных файла музыкального трека.</returns>
    private static Result<TrackFile>? CreateTrackFile(Dtos.TrackFile trackFile)
    {
        var trackFileNameResult = TrackFileName.Create(trackFile.FileName ?? string.Empty);
        if (trackFileNameResult.IsFailed)
        {
            return null;
        }

        var trackFileExtensionResult = TrackFileExtension.Create(trackFile.Extension ?? string.Empty);
        if (trackFileExtensionResult.IsFailed)
        {
            return null;
        }

        var trackFileMimeTypeResult = TrackFileMimeType.Create(trackFile.MimeType ?? string.Empty);
        if (trackFileMimeTypeResult.IsFailed)
        {
            return null;
        }

        var trackFileDataResult = TrackFileData.Create(trackFile.File?.ToArray() ?? Array.Empty<byte>());
        if (trackFileDataResult.IsFailed)
        {
            return null;
        }

        return TrackFile.Create(trackFileNameResult.Value, trackFileExtensionResult.Value,
            trackFileMimeTypeResult.Value,
            trackFileDataResult.Value);
    }
}