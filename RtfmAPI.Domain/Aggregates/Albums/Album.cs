using RftmAPI.Domain.Aggregates.Albums.ValueObjects;
using RftmAPI.Domain.Aggregates.Bands.ValueObjects;
using RftmAPI.Domain.Aggregates.Relations.TracksAlbums;
using RftmAPI.Domain.Aggregates.Tracks;
using RftmAPI.Domain.Aggregates.Tracks.ValueObjects;
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
    /// Дата выпуска альбома
    /// </summary>
    public DateTime ReleaseDate { get; private set; }

    // /// <summary>
    // /// Идентификатор музыкальной группы
    // /// </summary>
    // public BandId BandId { get; private set; }
    
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
    /// <param name="releaseDate">Дата выпуска альбома</param>
    public Album(string name, DateTime releaseDate) : base(AlbumId.Create())
    {
        Name = new AlbumName(name);
        ReleaseDate = releaseDate;

        _trackIds = new List<TrackId>();
    }
    
    /// <summary>
    /// Добавление трека
    /// </summary>
    /// <param name="trackId">Идентификатор трека</param>
    public void AddTrack(Track track)
    {
        var trackId = TrackId.Create(track.Id.Value);
        
        _trackIds.Add(trackId);
        // _tracks.Add(trackId);
    }
}