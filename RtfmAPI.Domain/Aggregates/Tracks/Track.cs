using System.Reflection.Metadata;
using RftmAPI.Domain.Aggregates.Albums;
using RftmAPI.Domain.Aggregates.Albums.ValueObjects;
using RftmAPI.Domain.Aggregates.Genres.ValueObjects;
using RftmAPI.Domain.Aggregates.Relations.TracksAlbums;
using RftmAPI.Domain.Aggregates.Tracks.ValueObjects;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Aggregates.Tracks;

/// <summary>
/// Музыкальный трек
/// </summary>
public sealed class Track : AggregateRoot<TrackId, Guid>
{
    private readonly List<GenreId> _genres;
    private readonly List<AlbumId> _albumIds;

    /// <summary>
    /// Наименование трека
    /// </summary>
    public TrackName Name { get; private set; }

    /// <summary>
    /// Музыкальный трек в виде байт
    /// </summary>
    public Blob Data { get; private set; }

    /// <summary>
    /// Дата выпуска
    /// </summary>
    public DateTime ReleaseDate { get; private set; }

    // /// <summary>
    // /// Жанры
    // /// </summary>
    // public IEnumerable<GenreId> Genres => _genres;

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
    /// <param name="data">Музыкальный трек в виде байт</param>
    /// <param name="releaseDate">Дата выпуска трека</param>
    public Track(string name, Blob data, DateTime releaseDate) : base(TrackId.Create())
    {
        Name = new TrackName(name);
        Data = data;
        ReleaseDate = releaseDate;

        _genres = new List<GenreId>();
        _albumIds = new List<AlbumId>();
    }

    /// <summary>
    /// Добавление музыкального жанра
    /// </summary>
    /// <param name="genreId">Идентификатор жанра</param>
    public void AddGenre(GenreId genreId)
    {
        _genres.Add(genreId);
    }
    
    public void AddAlbum(Album album)
    {
        var trackId = TrackId.Create(Id.Value);
        var albumId = AlbumId.Create(album.Id.Value);
        
        _albumIds.Add(albumId);
    }
}