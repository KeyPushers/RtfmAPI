using RftmAPI.Domain.Aggregates.Albums;
using RftmAPI.Domain.Aggregates.Albums.ValueObjects;
using RftmAPI.Domain.Aggregates.Tracks;
using RftmAPI.Domain.Aggregates.Tracks.ValueObjects;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.DomainNeeds.TrackAlbum;

public class TrackAlbum : AggregateRoot<TrackAlbumId, string>
{
    public TrackId TrackId { get; private init; }
     
    // public AlbumId AlbumId { get; private init; }

    private TrackAlbum() : base(TrackAlbumId.Create())
    {
        
    }
    
    public TrackAlbum(Track track, Album album) : base(TrackAlbumId.Create(track, album))
    {
        TrackId = track.Id as TrackId;
        // AlbumId = album.Id as AlbumId;
    }
}