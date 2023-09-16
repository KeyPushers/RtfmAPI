using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RftmAPI.Domain.Aggregates.Albums;
using RftmAPI.Domain.Aggregates.Albums.Repository;

namespace RtfmAPI.Application.Requests.Albums.Queries.GetAlbums;

public class GetAlbumsQueryHandler : IRequestHandler<GetAlbumsQuery, List<Album>>
{
    private readonly IAlbumsRepository _albumsRepository;

    public GetAlbumsQueryHandler(IAlbumsRepository albumsRepository)
    {
        _albumsRepository = albumsRepository;
    }
    
    public Task<List<Album>> Handle(GetAlbumsQuery request, CancellationToken cancellationToken)
    {
        return _albumsRepository.GetAlbumsAsync();
    }
}