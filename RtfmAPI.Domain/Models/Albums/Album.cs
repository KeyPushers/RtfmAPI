using RftmAPI.Domain.Exceptions.AlbumExceptions;
using RftmAPI.Domain.Models.Albums.Events;
using RftmAPI.Domain.Models.Albums.ValueObjects;
using RftmAPI.Domain.Models.Bands;
using RftmAPI.Domain.Models.Bands.ValueObjects;
using RftmAPI.Domain.Models.Tracks;
using RftmAPI.Domain.Models.Tracks.ValueObjects;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Albums;

/// <summary>
/// Музыкальный альбом.
/// </summary>
public sealed class Album : AggregateRoot<AlbumId, Guid>
{
    private readonly HashSet<TrackId> _trackIds = new();
    private readonly HashSet<BandId> _bandIds = new();

    /// <summary>
    /// Название музыкального альбома.
    /// </summary>
    public AlbumName Name { get; private set; }

    /// <summary>
    /// Дата выпуска музыкального альбома.
    /// </summary>
    public AlbumReleaseDate ReleaseDate { get; private set; }

    /// <summary>
    /// Музыкальные треки.
    /// </summary>
    public IReadOnlyCollection<TrackId> TrackIds => _trackIds;

    /// <summary>
    /// Музыкальные группы.
    /// </summary>
    public IReadOnlyCollection<BandId> BandIds => _bandIds;

    /// <summary>
    /// Создание музыкального альбома.
    /// </summary>
#pragma warning disable CS8618
    private Album()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Музыкальный альбом.
    /// </summary>
    /// <param name="name">Название музыкального альбома.</param>
    /// <param name="releaseDate">Дата выпуска музыкального альбома.</param>
    /// <param name="bands">Музыкальные группы.</param>
    /// <param name="tracks">Музыкальные треки.</param>
    private Album(AlbumName name, AlbumReleaseDate releaseDate, IEnumerable<Band> bands, IEnumerable<Track> tracks) :
        base(AlbumId.Create())
    {
        Name = name;
        ReleaseDate = releaseDate;
        _bandIds = bands.Select(band => BandId.Create(band.Id.Value)).ToHashSet();
        
        foreach (var track in tracks)
        {
            var trackId = TrackId.Create(track.Id.Value);
            _trackIds.Add(trackId);
            AddDomainEvent(new TrackAddedToAlbumDomainEvent(this, track));
        }
    }

    /// <summary>
    /// Создание музыкального альбома.
    /// </summary>
    /// <param name="name">Название музыкального альбома.</param>
    /// <param name="releaseDate">Дата выпуска музыкального альбома.</param>
    /// <returns>Музыкальный альбом.</returns>
    public static Result<Album> Create(AlbumName name, AlbumReleaseDate releaseDate)
    {
        return new Album(name, releaseDate, Enumerable.Empty<Band>(), Enumerable.Empty<Track>());
    }

    /// <summary>
    /// Создание музыкального альбома.
    /// </summary>
    /// <param name="name">Название музыкального альбома.</param>
    /// <param name="releaseDate">Дата выпуска музыкального альбома.</param>
    /// <param name="bands">Музыкальные группы.</param>
    /// <param name="tracks">Музыкальные треки.</param>
    /// <returns>Музыкальный альбом.</returns>
    public static Result<Album> Create(AlbumName name, AlbumReleaseDate releaseDate, IEnumerable<Band> bands,
        IEnumerable<Track> tracks)
    {
        return new Album(name, releaseDate, bands, tracks);
    }

    /// <summary>
    /// Добавление музыкального трека к музыкальному альбому.
    /// </summary>
    /// <param name="track">Музыкальный трек.</param>
    /// <returns>Результат операции.</returns>
    public BaseResult AddTrack(Track track)
    {
        var trackId = TrackId.Create(track.Id.Value);
        if (_trackIds.Add(trackId))
        {
            AddDomainEvent(new TrackAddedToAlbumDomainEvent(this, track));
        }
        
        return BaseResult.Success();
    }

    /// <summary>
    /// Удаление музыкального трека из музыкального альбома.
    /// </summary>
    /// <param name="track">Музыкальный трек.</param>
    /// <returns>Результат операции.</returns>
    public BaseResult RemoveTrack(Track track)
    {
        var trackId = TrackId.Create(track.Id.Value);

        if (!_trackIds.Contains(trackId))
        {
            return BaseResult.Success();
        }

        if (!_trackIds.Remove(trackId))
        {
            var albumId = AlbumId.Create(Id.Value);
            return AlbumExceptions.TrackRemovingFromAlbumFailedException(albumId, trackId);
        }
        
        AddDomainEvent(new TrackRemovedFromAlbumDomainEvent(this, trackId));
        return BaseResult.Success();
    }
}