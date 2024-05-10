using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using MediatR;
using RtfmAPI.Application.Interfaces.Persistence.Queries;
using RtfmAPI.Application.Requests.Tracks.Queries.GetTrackStream.Dtos;
using RtfmAPI.Domain.Models.Tracks.ValueObjects;

namespace RtfmAPI.Application.Requests.Tracks.Queries.GetTrackStream;

/// <summary>
/// Обработчик запроса потока музыкального трека.
/// </summary>
public class GetTrackStreamQueryHandler : IRequestHandler<GetTrackStreamQuery, Result<TrackStream>>
{
    private readonly ITracksQueriesRepository _tracksQueriesRepository;
    private readonly ITrackFilesQueriesRepository _trackFilesQueriesRepository;

    /// <summary>
    /// Создание обработчика запроса потока музыкального трека.
    /// </summary>
    /// <param name="tracksQueriesRepository">Репозиторий запросов музыкальных треков.</param>
    /// <param name="trackFilesQueriesRepository">Репозиторий запросов файлов музыкальных треков.</param>
    public GetTrackStreamQueryHandler(ITracksQueriesRepository tracksQueriesRepository,
        ITrackFilesQueriesRepository trackFilesQueriesRepository)
    {
        _tracksQueriesRepository = tracksQueriesRepository;
        _trackFilesQueriesRepository = trackFilesQueriesRepository;
    }

    /// <summary>
    /// Обработка запроса музыкального трека по идентификатору
    /// </summary>
    /// <param name="request">Запрос музыкального трека по идентификатору</param>
    /// <param name="cancellationToken">Токен отменеы</param>
    /// <returns>Музыкальный трек</returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<Result<TrackStream>> Handle(GetTrackStreamQuery request,
        CancellationToken cancellationToken = default)
    {
        var trackId = TrackId.Create(request.Id);

        var getTrackResult = await _tracksQueriesRepository.GetTrackByIdAsync(trackId);
        if (getTrackResult.IsFailed)
        {
            return getTrackResult.ToResult();
        }

        var track = getTrackResult.ValueOrDefault;

        if (track.TrackFileId is null)
        {
            throw new NotImplementedException();
        }

        var getTrackFileResult = await _trackFilesQueriesRepository.GetTrackFileByIdAsync(track.TrackFileId);
        if (getTrackFileResult.IsFailed)
        {
            return getTrackFileResult.ToResult();
        }

        var trackFile = getTrackFileResult.ValueOrDefault;

        var stream = new MemoryStream(trackFile.Data.Value);
        var mediaType = trackFile.MimeType.Value;

        return new TrackStream(stream, mediaType);
    }
}