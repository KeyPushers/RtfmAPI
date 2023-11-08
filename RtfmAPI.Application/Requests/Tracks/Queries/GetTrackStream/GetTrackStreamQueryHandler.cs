using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RftmAPI.Domain.Exceptions.TrackExceptions;
using RftmAPI.Domain.Exceptions.TrackFileExceptions;
using RftmAPI.Domain.Models.Tracks.ValueObjects;
using RftmAPI.Domain.Primitives;
using RtfmAPI.Application.Common.Interfaces.Persistence;
using RtfmAPI.Application.Requests.Tracks.Queries.GetTrackStream.Dtos;

namespace RtfmAPI.Application.Requests.Tracks.Queries.GetTrackStream;

/// <summary>
/// Обработчик запроса потока музыкального трека.
/// </summary>
public class GetTrackStreamQueryHandler : IRequestHandler<GetTrackStreamQuery, Result<TrackStream>>
{
    private readonly ITracksRepository _tracksRepository;
    private readonly ITrackFilesRepository _trackFilesRepository;

    /// <summary>
    /// Создание обработчика запроса потока музыкального трека.
    /// </summary>
    /// <param name="tracksRepository">Репозиторий музыкальных треков.</param>
    /// <param name="trackFilesRepository">Репозиторий файлов музыкальных треков.</param>
    public GetTrackStreamQueryHandler(ITracksRepository tracksRepository, ITrackFilesRepository trackFilesRepository)
    {
        _tracksRepository = tracksRepository;
        _trackFilesRepository = trackFilesRepository;
    }
    
    /// <summary>
    /// Обработка запроса музыкального трека по идентификатору
    /// </summary>
    /// <param name="request">Запрос музыкального трека по идентификатору</param>
    /// <param name="cancellationToken">Токен отменеы</param>
    /// <returns>Музыкальный трек</returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<Result<TrackStream>> Handle(GetTrackStreamQuery request, CancellationToken cancellationToken = default)
    {
        var trackId = TrackId.Create(request.Id);
        var track = await _tracksRepository.GetTrackByIdAsync(trackId);
        if (track is null)
        {
            return TrackExceptions.NotFound(trackId);
        }
        
        var trackFile = await _trackFilesRepository.GetTrackFileByIdAsync(track.TrackFileId);
        if (trackFile is null)
        {
            return TrackFileExceptions.NotFound(track.TrackFileId);
        }

        var stream = new MemoryStream(trackFile.Data.Value);
        var mediaType = trackFile.MimeType.Value;
        
        return new TrackStream(stream, mediaType);
    }
}