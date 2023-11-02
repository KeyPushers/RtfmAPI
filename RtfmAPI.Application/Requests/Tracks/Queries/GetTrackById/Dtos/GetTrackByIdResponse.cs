using System.IO;

namespace RtfmAPI.Application.Requests.Tracks.Queries.GetTrackById.Dtos;

public class GetTrackByIdResponse
{
    public Stream? Stream { get; init; }

    public string? MediaType { get; init; }
}