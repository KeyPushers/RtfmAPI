using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RftmAPI.Domain.Primitives;
using RtfmAPI.Application.Common.Interfaces.Persistence;
using RtfmAPI.Application.Requests.Albums.Queries.GetAlbumsItems.Dtos;

namespace RtfmAPI.Application.Requests.Albums.Queries.GetAlbumsItems;

/// <summary>
/// Обработчик запроса получения музыкальных альбомов.
/// </summary>
public class GetAlbumsItemsQueryHandler : IRequestHandler<GetAlbumsItemsQuery, Result<AlbumsItems>>
{
    private readonly IAlbumsRepository _albumsRepository;

    /// <summary>
    /// Создание обработчика запроса получения музыкальных альбомов.
    /// </summary>
    /// <param name="albumsRepository">Репозиторий музыкальных альбомов.</param>
    public GetAlbumsItemsQueryHandler(IAlbumsRepository albumsRepository)
    {
        _albumsRepository = albumsRepository;
    }

    /// <summary>
    /// Обработка запроса получения музыкальных альбомов.
    /// </summary>
    /// <param name="request">Запрос получения музыкальных альбомов.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Объект переноса данных музыкальных альбомов.</returns>
    public async Task<Result<AlbumsItems>> Handle(GetAlbumsItemsQuery request, CancellationToken cancellationToken)
    {
        var albums = await _albumsRepository.GetAlbumsAsync();

        return new AlbumsItems
        {
            Albums = albums.Select(entity => new AlbumItem
            {
                Id = entity.Id.Value,
                Name = entity.Name.Value,
                ReleaseDate = entity.ReleaseDate.Value
            }).ToList()
        };
    }
}