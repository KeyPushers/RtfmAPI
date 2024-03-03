using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RtfmAPI.Application.Interfaces.Persistence.Queries;
using RtfmAPI.Application.Requests.Albums.Queries.GetAlbumInfo.Dtos;
using RtfmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Application.Requests.Albums.Queries.GetAlbumInfo;

/// <summary>
/// Обработчик запроса информации о музыкальном альбоме.
/// </summary>
public class GetAlbumInfoQueryHandler : IRequestHandler<GetAlbumInfoQuery, Result<AlbumInfo>>
{
    private readonly IAlbumsQueriesRepository _repository;

    /// <summary>
    /// Создание обработчика запроса информации о музыкальном альбоме.
    /// </summary>
    /// <param name="repository">Репозиторий музыкальных альбомов.</param>
    public GetAlbumInfoQueryHandler(IAlbumsQueriesRepository repository)
    {
        _repository = repository;
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
        var getAlbumResult = await _repository.GetAlbumByIdAsync(id);
        if (getAlbumResult.IsFailed)
        {
            return getAlbumResult.Error;
        }

        var album = getAlbumResult.Value;
        
        return new AlbumInfo
        {
            Id = album.Id.Value,
            Name = album.Name.Value,
            ReleaseDate = album.ReleaseDate.Value
        };
    }
}