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
    private List<GenreId> _genreIds;

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
    public AlbumId AlbumId { get; private set; }

    /// <summary>
    /// Музыкальные жанры.
    /// </summary>
    public IReadOnlyCollection<GenreId> GenreIds => _genreIds;

    /// <summary>
    /// Музыкальный трек.
    /// </summary>
#pragma warning disable CS8618
    private Track()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Музыкальный трек.
    /// </summary>
    /// <param name="name">Название музыкального трека.</param>
    /// <param name="releaseDate">Дата выпуска музыкального трека.</param>
    /// <param name="trackFile">Содержимое файла музыкального трека.</param>
    /// <param name="album">Музыкальный альбом.</param>
    /// <param name="genres">Музыкальные жанры.</param>
    public Track(TrackName name, TrackReleaseDate releaseDate, TrackFile trackFile, Album album,
        IEnumerable<Genre> genres) : base(TrackId.Create())
    {
        Name = name;
        ReleaseDate = releaseDate;
        TrackFile = trackFile;
        AlbumId = (AlbumId) album.Id;
        _genreIds = genres.Select(genre => (GenreId) genre.Id).ToList();
    }
}