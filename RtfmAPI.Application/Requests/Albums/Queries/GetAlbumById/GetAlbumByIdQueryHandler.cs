using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RftmAPI.Domain.Models.Albums;
using RftmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Application.Common.Interfaces.Persistence;

namespace RtfmAPI.Application.Requests.Albums.Queries.GetAlbumById;

/// <summary>
/// Обработчик запроса музыкального альбома по идентификатору.
/// </summary>
public class GetAlbumByIdQueryHandler : IRequestHandler<GetAlbumByIdQuery, Album?>
{
    private readonly IAlbumsRepository _albumsRepository;

    /// <summary>
    /// Обработчик запроса музыкального альбома по идентификатору.
    /// </summary>
    /// <param name="albumsRepository">Репозиторий музыкальных альбомов.</param>
    public GetAlbumByIdQueryHandler(IAlbumsRepository albumsRepository)
    {
        _albumsRepository = albumsRepository;
    }
    
    /// <summary>
    /// Обработка запроса музыкального альбома по идентификатору.
    /// </summary>
    /// <param name="request">Запрос музыкального альбома по идентификатору.</param>
    /// <param name="cancellationToken">Токен отменеы.</param>
    /// <returns>Музыкальный альбом.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<Album?> Handle(GetAlbumByIdQuery request, CancellationToken cancellationToken = default)
    {
        return _albumsRepository.GetAlbumByIdAsync(AlbumId.Create(request.Id));
    }
}