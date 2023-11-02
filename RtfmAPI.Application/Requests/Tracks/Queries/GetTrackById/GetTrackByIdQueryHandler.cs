using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RftmAPI.Domain.Models.Tracks;
using RftmAPI.Domain.Models.Tracks.ValueObjects;
using RftmAPI.Domain.Primitives;
using RtfmAPI.Application.Common.Interfaces.Persistence;
using RtfmAPI.Application.Requests.Tracks.Queries.GetTrackById.Dtos;

namespace RtfmAPI.Application.Requests.Tracks.Queries.GetTrackById;

/// <summary>
/// Обработчик запроса музыкального трека по идентификатору
/// </summary>
public class GetTrackByIdQueryHandler : IRequestHandler<GetTrackByIdQuery, Result<GetTrackByIdResponse?>>
{
    private readonly ITracksRepository _tracksRepository;
    private readonly ITrackFilesRepository _trackFilesRepository;

    /// <summary>
    /// Обработчик запроса музыкального трека по идентификатору
    /// </summary>
    /// <param name="tracksRepository">Репозиторий музыкальных треков.</param>
    /// <param name="trackFilesRepository">Репозиторий файлов музыкальных треков.</param>
    public GetTrackByIdQueryHandler(ITracksRepository tracksRepository, ITrackFilesRepository trackFilesRepository)
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
    public async Task<Result<GetTrackByIdResponse?>> Handle(GetTrackByIdQuery request, CancellationToken cancellationToken = default)
    {
        var track = await _tracksRepository.GetTrackByIdAsync(TrackId.Create(request.Id));
        if (track is null)
        {
            throw new ArgumentNullException();
        }
        
        var trackFile = await _trackFilesRepository.GetTrackFileByIdAsync(track.TrackFileId);
        if (trackFile is null)
        {
            throw new ArgumentNullException();
        }
        
        return new GetTrackByIdResponse
        {
            Stream = new MemoryStream(trackFile.Data.Value),
            MediaType = trackFile.MimeType.Value
        };
    }
}