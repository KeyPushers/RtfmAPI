using RftmAPI.Domain.Aggregates.Albums;
using RftmAPI.Domain.Aggregates.Albums.ValueObjects;
using RftmAPI.Domain.Aggregates.Tracks.ValueObjects;
using RftmAPI.Domain.DomainNeeds.TrackAlbum;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Aggregates.Tracks;

/// <summary>
/// Музыкальный трек
/// </summary>
public sealed class Track : AggregateRoot<TrackId, Guid>
{
    private readonly List<AlbumId> _albumIds;
    
    /// <summary>
    /// Наименование трека
    /// </summary>
    public TrackName Name { get; private set; }
    
    /// <summary>
    /// Альбомы
    /// </summary>
    public IEnumerable<AlbumId> AlbumIds => _albumIds;

    /// <summary>
    /// Музыкальный трек
    /// </summary>
#pragma warning disable CS8618
    private Track() : base(TrackId.Create())
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Музыкальный трек
    /// </summary>
    /// <param name="name">Наименование музыкального трека</param>
    public Track(string name) : base(TrackId.Create())
    {
        Name = new TrackName(name); 

        _albumIds = new List<AlbumId>();
    }

    public void AddAlbum(Album album)
    {
        // var trackAlbum = new TrackAlbum(this, album);
        // if (trackAlbum.Id is not TrackAlbumId trackAlbumId)
        // {
        //     return;
        // }
        //
        // if (_albumIds.Any(id => id.Value == trackAlbum.Id.Value))
        // {
        //     return;
        // }
        
        // _albumIds.Add(trackAlbumId);
        // album.AddTrack(this);
        
        if (album.Id is not AlbumId albumId)
        {
            return;
        }
        
        if (_albumIds.Any(id => id.Value == album.Id.Value))
        {
            return;
        }
        _albumIds.Add(albumId);
        album.AddTrack(this);
    }
}