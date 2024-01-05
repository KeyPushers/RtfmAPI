using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RftmAPI.Domain.Exceptions.AlbumExceptions;
using RftmAPI.Domain.Models.Albums.ValueObjects;
using RftmAPI.Domain.Primitives;
using RtfmAPI.Application.Common.Interfaces.Persistence;
using RtfmAPI.Application.Requests.Albums.Queries.GetAlbumInfo.Dtos;

namespace RtfmAPI.Application.Requests.Albums.Queries.GetAlbumInfo;

/// <summary>
/// Обработчик запроса информации о музыкальном альбоме.
/// </summary>
public class GetAlbumInfoQueryHandler : IRequestHandler<GetAlbumInfoQuery, Result<AlbumInfo>>
{
    private readonly IAlbumsRepository _albumsRepository;

    /// <summary>
    /// Создание обработчика запроса информации о музыкальном альбоме.
    /// </summary>
    /// <param name="albumsRepository">Репозиторий музыкальных альбомов.</param>
    public GetAlbumInfoQueryHandler(IAlbumsRepository albumsRepository)
    {
        _albumsRepository = albumsRepository;
    }
    
    /// <summary>
    /// Обработка запроса информации о музыкальном альбоме.
    /// </summary>
    /// <param name="request">Запрос информации о музыкальном альбоме.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Музыкальный альбом.</returns>
    public async Task<Result<AlbumInfo>> Handle(GetAlbumInfoQuery request, CancellationToken cancellationToken = default)
    {
        var id = AlbumId.Create(request.Id);
        var album = await _albumsRepository.GetAlbumByIdAsync(id);
        if (album is null)
        {
            return AlbumExceptions.NotFound(id);
        }
        
        return new AlbumInfo
        {
            Id = album.Id.Value,
            Name = album.Name.Value,
            ReleaseDate = album.ReleaseDate.Value
        };
    }
}