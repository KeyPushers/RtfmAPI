using MediatR;
using RftmAPI.Domain.Primitives;
using RtfmAPI.Application.Requests.Albums.Queries.GetAlbumsItems.Dtos;

namespace RtfmAPI.Application.Requests.Albums.Queries.GetAlbumsItems;

/// <summary>
/// Запрос получения музыкальных альбомов.
/// </summary>
public class GetAlbumsItemsQuery : IRequest<Result<AlbumsItems>>
{
    
}