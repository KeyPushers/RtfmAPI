using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RftmAPI.Domain.Models.Albums;
using RtfmAPI.Application.Common.Interfaces.Persistence;

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