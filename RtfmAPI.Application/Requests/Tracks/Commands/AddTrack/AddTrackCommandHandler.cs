using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RftmAPI.Domain.Aggregates.Tracks;

namespace RtfmAPI.Application.Requests.Tracks.Commands.AddTrack;

/// <summary>
/// Обработчик команды добавления музыкального трека
/// </summary>
public class AddTrackCommandHandler : IRequestHandler<AddTrackCommand, Track>
{
    private readonly ITrackRepository _trackRepository;

    /// <summary>
    /// Обработчик команды добавления музыкального трека
    /// </summary>
    /// <param name="trackRepository">Репозиторий музыкальных треков</param>
    public AddTrackCommandHandler(ITrackRepository trackRepository)
    {
        _trackRepository = trackRepository;
    }
    
    /// <summary>
    /// Обработка команды добавления музыкального трека
    /// </summary>
    /// <param name="request">Команда добавления музыкального трека</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Музыкальный трек</returns>
    public async Task<Track> Handle(AddTrackCommand request, CancellationToken cancellationToken = default)
    {
        var track = new Track(request.Name ?? "Не указано название", request.Data, request.ReleaseDate, Enumerable.Empty<GenreId>(), Enumerable.Empty<AlbumId>());

        await _trackRepository.AddAsync(track).ConfigureAwait(false);

        return track;
    }
}