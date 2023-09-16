using RftmAPI.Domain.Aggregates.Albums;
using RftmAPI.Domain.Aggregates.Tracks;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.DomainNeeds.TrackAlbum;

public class TrackAlbumId : AggregateRootId<string>
{
    public override string Value { get; }


#pragma warning disable CS8618
    private TrackAlbumId()
    {
    }
#pragma warning restore CS8618

    private TrackAlbumId(string value)
    {
        Value = value;
    }

    public static TrackAlbumId Create()
    {
        return new TrackAlbumId(Guid.NewGuid().ToString());
    }

    public static TrackAlbumId Create(string id)
    {
        return new TrackAlbumId(id);
    }

    public static TrackAlbumId Create(Track track, Album album)
    {
        return Create($"{track.Id.Value}-{album.Id.Value}");
    }
}