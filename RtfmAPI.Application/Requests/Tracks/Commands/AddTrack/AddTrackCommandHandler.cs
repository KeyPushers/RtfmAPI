using System;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using MediatR;
using RtfmAPI.Application.Fabrics.TrackFiles;
using RtfmAPI.Application.Fabrics.Tracks;
using RtfmAPI.Application.Interfaces.AudioHandlers;
using RtfmAPI.Application.Interfaces.Persistence.Commands;
using RtfmAPI.Application.Requests.Tracks.Commands.AddTrack.Dtos;
using RtfmAPI.Domain.Models.TrackFiles;

namespace RtfmAPI.Application.Requests.Tracks.Commands.AddTrack;

/// <summary>
/// Обработчик команды добавления музыкального трека
/// </summary>
public class AddTrackCommandHandler : IRequestHandler<AddTrackCommand, Result<AddedTrack>>
{
    private readonly ITracksCommandsRepository _tracksCommandsRepository;
    private readonly ITrackFilesCommandsRepository _trackFilesCommandsRepository;
    private readonly IAudioHandlerFactory _audioHandlerFactory;

    /// <summary>
    /// Обработчик команды добавления музыкального трека.
    /// </summary>
    /// <param name="tracksCommandsRepository">Репозиторий команд музыкальных треков.</param>
    /// <param name="trackFilesCommandsRepository">Репозиторий команд файлов музыкальных треков.</param>
    /// <param name="audioHandlerFactory">Интерфейс фабрики обработчика аудиофайлов.</param>
    public AddTrackCommandHandler(ITracksCommandsRepository tracksCommandsRepository,
        ITrackFilesCommandsRepository trackFilesCommandsRepository, IAudioHandlerFactory audioHandlerFactory)
    {
        _tracksCommandsRepository = tracksCommandsRepository;
        _trackFilesCommandsRepository = trackFilesCommandsRepository;
        _audioHandlerFactory = audioHandlerFactory;
    }

    /// <summary>
    /// Обработка команды добавления музыкального трека.
    /// </summary>
    /// <param name="request">Команда добавления музыкального трека.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Музыкальный трек.</returns>
    public async Task<Result<AddedTrack>> Handle(AddTrackCommand request, CancellationToken cancellationToken = default)
    {
        if (request.TrackFile is null)
        {
            throw new NotImplementedException();
        }

        var createTrackFileResult = CreateTrackFile(request.TrackFile);
        if (createTrackFileResult.IsFailed)
        {
            return createTrackFileResult.ToResult();
        }

        var trackFile = createTrackFileResult.ValueOrDefault;

        var tracksFactory = new TracksFactory(new()
        {
            Name = request.Name,
            ReleaseDate = request.ReleaseDate,
            TrackFileId = trackFile.Id.Value,
        });

        var createTrackResult = tracksFactory.Create();
        if (createTrackResult.IsFailed)
        {
            return createTrackResult.ToResult();
        }

        var track = createTrackResult.ValueOrDefault;

        await _trackFilesCommandsRepository.CommitChangesAsync(trackFile, cancellationToken);
        await _tracksCommandsRepository.CommitChangesAsync(track, cancellationToken);

        return new AddedTrack
        {
            Id = track.Id.Value,
            Name = track.Name.Value,
            Duration = trackFile.Duration.Value,
            ReleaseDate = track.ReleaseDate.Value
        };
    }

    /// <summary>
    /// Создание представления данных файла музыкального трека (<see cref="TrackFile"/>).
    /// </summary>
    /// <param name="trackFile">Объект переноса данных файла музыкального трека.</param>
    /// <returns>Представление данных файла музыкального трека.</returns>
    private Result<TrackFile> CreateTrackFile(AddingTrack trackFile)
    {
        var audioHandler = _audioHandlerFactory.Create(trackFile.File, trackFile.MimeType ?? string.Empty);
        if (audioHandler is null)
        {
            throw new NotImplementedException();
        }

        var trackFilesFactory = new TrackFilesFactory(new()
        {
            Name = trackFile.FileName,
            Data = trackFile.File.ToArray(),
            Extension = trackFile.Extension,
            MimeType = trackFile.MimeType,
            Duration = audioHandler.GetDuration()
        });

        return trackFilesFactory.Create();
    }
}