using RftmAPI.Domain.Aggregates.Albums;
using RftmAPI.Domain.Aggregates.Albums.ValueObjects;
using RftmAPI.Domain.Aggregates.Tracks.ValueObjects;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Aggregates.Tracks;

/// <summary>
/// Музыкальный трек
/// </summary>
public sealed class Track : AggregateRoot<TrackId, Guid>
{
    private readonly List<AlbumId> _albumIds = new();
    
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
    private Track()
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
    }

    public void AddAlbum(Album album)
    {
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