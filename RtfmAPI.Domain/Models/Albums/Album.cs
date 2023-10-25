using FluentResults;
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
    private readonly List<TrackId> _trackIds = new();
    private readonly List<BandId> _bandIds = new();

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
    /// Создание музыкального альбома.
    /// </summary>
    /// <param name="name">Название музыкального альбома.</param>
    /// <param name="releaseDate">Дата выпуска музыкального альбома.</param>
    private Album(AlbumName name, AlbumReleaseDate releaseDate) : base(AlbumId.Create())
    {
        Name = name;
        ReleaseDate = releaseDate;
    }

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
        _bandIds = bands.Select(band => (BandId) band.Id).ToList();
        _trackIds = tracks.Select(track => (TrackId) track.Id).ToList();
    }

    /// <summary>
    /// Создание музыкального альбома.
    /// </summary>
    /// <param name="name">Название музыкального альбома.</param>
    /// <param name="releaseDate">Дата выпуска музыкального альбома.</param>
    /// <returns>Музыкальный альбом.</returns>
    public static Result<Album> Create(AlbumName name, AlbumReleaseDate releaseDate)
    {
        return new Album(name, releaseDate);
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
}