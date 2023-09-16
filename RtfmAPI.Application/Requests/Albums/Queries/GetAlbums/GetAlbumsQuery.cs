using System.Collections.Generic;
using MediatR;
using RftmAPI.Domain.Aggregates.Albums;

namespace RtfmAPI.Application.Requests.Albums.Queries.GetAlbums;

public class GetAlbumsQuery : IRequest<List<Album>>
{
    
}