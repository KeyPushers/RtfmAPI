using RftmAPI.Domain.Aggregates.Albums.ValueObjects;
using RftmAPI.Domain.Aggregates.Tracks;
using RftmAPI.Domain.Aggregates.Tracks.ValueObjects;
using RftmAPI.Domain.DomainNeeds.TrackAlbum;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Aggregates.Albums;

/// <summary>
/// Альбом
/// </summary>
public sealed class Album : AggregateRoot<AlbumId, Guid>
{
    private readonly List<TrackId> _trackIds;

    /// <summary>
    /// Наименование альбома
    /// </summary>
    public AlbumName Name { get; private set; }

    /// <summary>
    /// Музыкальные треки
    /// </summary>
    public IEnumerable<TrackId> TrackIds => _trackIds;

    /// <summary>
    /// Альбом
    /// </summary>
#pragma warning disable CS8618
    private Album() : base(AlbumId.Create())
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Альбом
    /// </summary>
    /// <param name="name">Наименование альбома</param>
    public Album(string name) : base(AlbumId.Create())
    {
        Name = new AlbumName(name);

        _trackIds = new List<TrackId>();
    }
    
    public void AddTrack(Track track)
    {
        // var trackAlbum = new TrackAlbum(track, this);
        // if (trackAlbum.Id is not TrackAlbumId trackAlbumId)
        // {
        //     return;
        // }
        //
        // if (_trackIds.Any(id => id.Value == trackAlbum.Id.Value))
        // {
        //     return;
        // }
        //
        // _trackIds.Add(trackAlbumId);
        // track.AddAlbum(this);
        
        if (track.Id is not TrackId trackId)
        {
            return;
        }
        
        if (_trackIds.Any(id => id.Value == track.Id.Value))
        {
            return;
        }
        _trackIds.Add(trackId);
        track.AddAlbum(this);
    }
}