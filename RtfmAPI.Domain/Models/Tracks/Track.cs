using FluentResults;
using RftmAPI.Domain.Models.Albums;
using RftmAPI.Domain.Models.Albums.ValueObjects;
using RftmAPI.Domain.Models.Genres;
using RftmAPI.Domain.Models.Genres.ValueObjects;
using RftmAPI.Domain.Models.Tracks.Entities;
using RftmAPI.Domain.Models.Tracks.ValueObjects;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Tracks;

/// <summary>
/// Музыкальный трек.
/// </summary>
public sealed class Track : AggregateRoot<TrackId, Guid>
{
    private List<GenreId> _genreIds = new();

    /// <summary>
    /// Название музыкального трека.
    /// </summary>
    public TrackName Name { get; private set; }

    /// <summary>
    /// Дата выпуска музыкального трека.
    /// </summary>
    public TrackReleaseDate ReleaseDate { get; private set; }

    /// <summary>
    /// Содержимое файла музыкального трека.
    /// </summary>
    public TrackFile TrackFile { get; private set; }

    /// <summary>
    /// Музыкальный альбом.
    /// </summary>
    public AlbumId? AlbumId { get; private set; }

    /// <summary>
    /// Музыкальные жанры.
    /// </summary>
    public IReadOnlyCollection<GenreId> GenreIds => _genreIds;

    /// <summary>
    /// Создание музыкального трека.
    /// </summary>
#pragma warning disable CS8618
    private Track()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Создание музыкального трека.
    /// </summary>
    /// <param name="name">Название музыкального трека.</param>
    /// <param name="releaseDate">Дата выпуска музыкального трека.</param>
    /// <param name="trackFile">Содержимое файла музыкального трека.</param>
    private Track(TrackName name, TrackReleaseDate releaseDate, TrackFile trackFile) : base(TrackId.Create())
    {
        Name = name;
        ReleaseDate = releaseDate;
        TrackFile = trackFile;
    }
    
    /// <summary>
    /// Создание музыкального трека.
    /// </summary>
    /// <param name="name">Название музыкального трека.</param>
    /// <param name="releaseDate">Дата выпуска музыкального трека.</param>
    /// <param name="trackFile">Содержимое файла музыкального трека.</param>
    /// <param name="album">Музыкальный альбом.</param>
    /// <param name="genres">Музыкальные жанры.</param>
    private Track(TrackName name, TrackReleaseDate releaseDate, TrackFile trackFile, Album album,
        IEnumerable<Genre> genres) : base(TrackId.Create())
    {
        Name = name;
        ReleaseDate = releaseDate;
        TrackFile = trackFile;
        AlbumId = (AlbumId) album.Id;
        _genreIds = genres.Select(genre => (GenreId) genre.Id).ToList();
    }

    /// <summary>
    /// Создание музыкального трека.
    /// </summary>
    /// <param name="name">Название музыкального трека.</param>
    /// <param name="releaseDate">Дата выпуска музыкального трека.</param>
    /// <param name="trackFile">Содержимое файла музыкального трека.</param>
    /// <returns>Музыкальный трек.</returns>
    public static Result<Track> Create(TrackName name, TrackReleaseDate releaseDate, TrackFile trackFile)
    {
        return new Track(name, releaseDate, trackFile);
    }
    
    /// <summary>
    /// Создание музыкального трека.
    /// </summary>
    /// <param name="name">Название музыкального трека.</param>
    /// <param name="releaseDate">Дата выпуска музыкального трека.</param>
    /// <param name="trackFile">Содержимое файла музыкального трека.</param>
    /// <param name="album">Музыкальный альбом.</param>
    /// <param name="genres">Музыкальные жанры.</param>
    /// <returns>Музыкальный трек.</returns>
    public static Result<Track> Create(TrackName name, TrackReleaseDate releaseDate, TrackFile trackFile, Album album, IEnumerable<Genre> genres)
    {
        return new Track(name, releaseDate, trackFile, album, genres);
    }
}