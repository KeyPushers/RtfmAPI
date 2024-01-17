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
    private Album(AlbumName name, AlbumReleaseDate releaseDate, IReadOnlyCollection<Band> bands,
        IReadOnlyCollection<Track> tracks) : base(AlbumId.Create())
    {
        Name = name;
        AddDomainEvent(new AlbumNameChangedDomainEvent(this, name));

        ReleaseDate = releaseDate;
        AddDomainEvent(new AlbumReleaseDateChangedDomainEvent(this, releaseDate));

        _trackIds = tracks.Select(track => TrackId.Create(track.Id.Value)).ToHashSet();
        AddDomainEvent(new TracksAddedToAlbumDomainEvent(this, tracks));

        _bandIds = bands.Select(band => BandId.Create(band.Id.Value)).ToHashSet();
        AddDomainEvent(new BandsAddedToAlbumDomainEvent(this, bands));
    }

    /// <summary>
    /// Создание музыкального альбома.
    /// </summary>
    /// <param name="name">Название музыкального альбома.</param>
    /// <param name="releaseDate">Дата выпуска музыкального альбома.</param>
    /// <returns>Музыкальный альбом.</returns>
    public static Result<Album> Create(AlbumName name, AlbumReleaseDate releaseDate)
    {
        return new Album(name, releaseDate, new List<Band>(), new List<Track>());
    }

    /// <summary>
    /// Создание музыкального альбома.
    /// </summary>
    /// <param name="name">Название музыкального альбома.</param>
    /// <param name="releaseDate">Дата выпуска музыкального альбома.</param>
    /// <param name="bands">Музыкальные группы.</param>
    /// <param name="tracks">Музыкальные треки.</param>
    /// <returns>Музыкальный альбом.</returns>
    public static Result<Album> Create(AlbumName name, AlbumReleaseDate releaseDate, IReadOnlyCollection<Band> bands,
        IReadOnlyCollection<Track> tracks)
    {
        return new Album(name, releaseDate, bands, tracks);
    }

    /// <summary>
    /// Изменение названия музыкального альбома.
    /// </summary>
    /// <param name="name">Название альбома.</param>
    public BaseResult SetName(AlbumName name)
    {
        if (Name == name)
        {
            return BaseResult.Success();
        }

        Name = name;
        AddDomainEvent(new AlbumNameChangedDomainEvent(this, name));
        return BaseResult.Success();
    }

    /// <summary>
    /// Изменение даты выпуска музыкального альбома.
    /// </summary>
    /// <param name="releaseDate">Дата выпуска музыкального альбома.</param>
    public BaseResult SetReleaseDate(AlbumReleaseDate releaseDate)
    {
        if (ReleaseDate == releaseDate)
        {
            return BaseResult.Success();
        }

        ReleaseDate = releaseDate;
        AddDomainEvent(new AlbumReleaseDateChangedDomainEvent(this, releaseDate));
        return BaseResult.Success();
    }
    
    /// <summary>
    /// Добавление музыкальных треков.
    /// </summary>
    /// <param name="tracks">Добавляемые музыкальные треки.</param>
    public BaseResult AddTracks(IReadOnlyCollection<Track> tracks)
    {
        if (!tracks.Any())
        {
            return BaseResult.Success();
        }

        List<Track> addedTracks = new();
        foreach (var track in tracks)
        {
            var trackId = TrackId.Create(track.Id.Value);
            if (_trackIds.Contains(trackId))
            {
                continue;
            }

            addedTracks.Add(track);
            _trackIds.Add(trackId);
        }

        AddDomainEvent(new TracksAddedToAlbumDomainEvent(this, addedTracks));
        return BaseResult.Success();
    }

    /// <summary>
    /// Удаление музыкальных треков.
    /// </summary>
    /// <param name="tracks">Удаляемые музыкальные треки.</param>
    public BaseResult RemoveTracks(IReadOnlyCollection<Track> tracks)
    {
        if (!tracks.Any())
        {
            return BaseResult.Success();
        }

        List<Track> removedTracks = new();
        foreach (var track in tracks)
        {
            var trackId = TrackId.Create(track.Id.Value);
            if (!_trackIds.Contains(trackId))
            {
                continue;
            }

            removedTracks.Add(track);
            _trackIds.Remove(trackId);
        }

        AddDomainEvent(new TracksRemovedFromAlbumDomainEvent(this, removedTracks));
        return BaseResult.Success();
    }

    /// <summary>
    /// Добавление музыкальных групп.
    /// </summary>
    /// <param name="bands">Добавляемые музыкальные группы.</param>
    public BaseResult AddBands(IReadOnlyCollection<Band> bands)
    {
        if (!bands.Any())
        {
            return BaseResult.Success();
        }

        List<Band> addedBands = new();
        foreach (var band in bands)
        {
            var bandId = BandId.Create(band.Id.Value);
            if (_bandIds.Contains(bandId))
            {
                continue;
            }

            addedBands.Add(band);
            _bandIds.Add(bandId);
        }

        AddDomainEvent(new BandsAddedToAlbumDomainEvent(this, addedBands));
        return BaseResult.Success();
    }

    /// <summary>
    /// Удаление музыкальных групп.
    /// </summary>
    /// <param name="bands">Удаляемые музыкальные группы.</param>
    public BaseResult RemoveBands(IReadOnlyCollection<Band> bands)
    {
        if (!bands.Any())
        {
            return BaseResult.Success();
        }

        List<Band> removedBands = new();
        foreach (var band in bands)
        {
            var bandId = BandId.Create(band.Id.Value);
            if (!_bandIds.Contains(bandId))
            {
                continue;
            }

            removedBands.Add(band);
            _bandIds.Remove(bandId);
        }

        AddDomainEvent(new BandsRemovedFromAlbumDomainEvent(this, removedBands));
        return BaseResult.Success();
    }

    /// <summary>
    /// Удаление альбома.
    /// </summary>
    public BaseResult Delete()
    {
        var albumId = AlbumId.Create(Id.Value);
        AddDomainEvent(new AlbumDeletedDomainEvent(albumId));
        return BaseResult.Success();
    }
}