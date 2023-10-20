using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RftmAPI.Domain.Models.Albums.Repository;
using RftmAPI.Domain.Models.Tracks;
using RftmAPI.Domain.Models.Tracks.Repository;
using RftmAPI.Domain.Models.Tracks.ValueObjects;

namespace RtfmAPI.Application.Requests.Tracks.Commands.AddTrack;

/// <summary>
/// Обработчик команды добавления музыкального трека
/// </summary>
public class AddTrackCommandHandler : IRequestHandler<AddTrackCommand, Track>
{
    private readonly ITracksRepository _tracksRepository;
    private readonly IAlbumsRepository _albumsRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Обработчик команды добавления музыкального трека
    /// </summary>
    /// <param name="tracksRepository">Репозиторий музыкальных треков</param>
    /// <param name="albumsRepository">Репозиторий альбомов</param>
    /// <param name="unitOfWork">Единица работы</param>
    public AddTrackCommandHandler(ITracksRepository tracksRepository, IAlbumsRepository albumsRepository,
        IUnitOfWork unitOfWork)
    {
        _tracksRepository = tracksRepository;
        _albumsRepository = albumsRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Обработка команды добавления музыкального трека
    /// </summary>
    /// <param name="request">Команда добавления музыкального трека</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Музыкальный трек</returns>
    public async Task<Track> Handle(AddTrackCommand request, CancellationToken cancellationToken = default)
    {
        var trackNameResult = TrackName.Create(request.Name ?? "Не указано название");
        if (trackNameResult.IsFailure)
        {
            throw new NotImplementedException();
        }
        
        var track = new Track(trackNameResult.Value);

        return track;
    }
}