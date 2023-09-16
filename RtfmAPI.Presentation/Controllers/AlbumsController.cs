using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RftmAPI.Domain.Aggregates.Albums;
using RtfmAPI.Application.Requests.Albums.Queries.GetAlbums;

namespace RtfmAPI.Presentation.Controllers;

public class AlbumsController : ApiControllerBase
{
    public AlbumsController(IMediator mediator) : base(mediator)
    {
    }
    

    [HttpGet]
    public Task<List<Album>> GetAlbumsAsync(CancellationToken cancellationToken =default)
    {
        var query = new GetAlbumsQuery();

        return Mediator.Send(query, cancellationToken);
    }
}