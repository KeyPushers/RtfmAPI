using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RftmAPI.Domain.Models.Albums.ValueObjects;
using RftmAPI.Domain.Models.Tracks.ValueObjects;
using RftmAPI.Domain.Primitives;
using RtfmAPI.Application.Common.Interfaces.Persistence;

namespace RtfmAPI.Application.Requests.Tracks.Commands.AddAlbumToTrack;

/// <summary>
/// Обработчик команды добавления музыкального альбома к музыкальному треку.
/// </summary>
public class AddAlbumToTrackCommandHandler : IRequestHandler<AddAlbumToTrackCommand, Result<Unit>>
{
    private readonly ITracksRepository _tracksRepository;
    private readonly IAlbumsRepository _albumsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddAlbumToTrackCommandHandler(ITracksRepository tracksRepository, IAlbumsRepository albumsRepository, IUnitOfWork unitOfWork)
    {
        _tracksRepository = tracksRepository;
        _albumsRepository = albumsRepository;
        _unitOfWork = unitOfWork;
    }
    
    /// <summary>
    /// Обработка команды добавления музыкального альбома к музыкальному треку.
    /// </summary>
    /// <param name="request">Команда добавления музыкального альбома к музыкальному треку.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns><see cref="Unit"/>.</returns>
    public async Task<Result<Unit>> Handle(AddAlbumToTrackCommand request, CancellationToken cancellationToken = default)
    {
        var track = await _tracksRepository.GetTrackByIdAsync(TrackId.Create(request.TrackId));
        if (track is null)
        {
            return new NullReferenceException(nameof(track));
        }

        var album = await _albumsRepository.GetAlbumByIdAsync(AlbumId.Create(request.AlbumId));
        if (album is null)
        {
            return new NullReferenceException(nameof(album));
        }

        var addAlbumToTrackResult = track.AddAlbum(album);
        if (addAlbumToTrackResult.IsFailed)
        {
            return addAlbumToTrackResult.Error;
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}