using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RftmAPI.Domain.Aggregates.Albums;
using RftmAPI.Domain.Aggregates.Albums.Repository;
using RftmAPI.Domain.Aggregates.Tracks;
using RftmAPI.Domain.Aggregates.Tracks.Repository;

namespace RtfmAPI.Application.Requests.Tracks.Commands.AddTrack;

/// <summary>
/// Обработчик команды добавления музыкального трека
/// </summary>
public class AddTrackCommandHandler : IRequestHandler<AddTrackCommand, Track>
{
    private readonly ITracksRepository _tracksRepository;
    private readonly IAlbumsRepository _albumsRepository;

    /// <summary>
    /// Обработчик команды добавления музыкального трека
    /// </summary>
    /// <param name="tracksRepository">Репозиторий музыкальных треков</param>
    /// <param name="albumsRepository">Репозиторий альбомов</param>
    public AddTrackCommandHandler(ITracksRepository tracksRepository, IAlbumsRepository albumsRepository)
    {
        _tracksRepository = tracksRepository;
        _albumsRepository = albumsRepository;
    }
    
    /// <summary>
    /// Обработка команды добавления музыкального трека
    /// </summary>
    /// <param name="request">Команда добавления музыкального трека</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Музыкальный трек</returns>
    public async Task<Track> Handle(AddTrackCommand request, CancellationToken cancellationToken = default)
    {
        var track = new Track(request.Name ?? "Не указано название", request.Data, request.ReleaseDate);

        var album = new Album(request.Name ?? "Не указано название", DateTime.Now);
        
        track.AddAlbum(album);
        album.AddTrack(track);
        
        await _albumsRepository.AddAsync(album).ConfigureAwait(false);
        
        await _tracksRepository.AddAsync(track).ConfigureAwait(false);

        var tracks = await _tracksRepository.GetTracksAsync().ConfigureAwait(false);
        
        var albums = await _albumsRepository.GetAlbumsAsync().ConfigureAwait(false);
        
        return track;
    }
}