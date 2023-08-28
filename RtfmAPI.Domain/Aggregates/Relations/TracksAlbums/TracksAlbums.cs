using RftmAPI.Domain.Aggregates.Albums.ValueObjects;
using RftmAPI.Domain.Aggregates.Tracks.ValueObjects;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Aggregates.Relations.TracksAlbums;

public class TracksAlbums : AggregateRoot<TracksAlbumsId, Guid>
{
#pragma warning disable CS8618
    private TracksAlbums() : base(TracksAlbumsId.Create())
    {
    }
#pragma warning disable CS8618

    public TracksAlbums(TrackId trackId, AlbumId albumId) : base(TracksAlbumsId.Create())
    {
        TrackId = trackId;
        AlbumId = albumId;
    }

    public TrackId TrackId { get; private init; }

    public AlbumId AlbumId { get; private init; }
}