using FluentResults;
using RftmAPI.Domain.Models.Albums;
using RftmAPI.Domain.Models.Albums.ValueObjects;
using RftmAPI.Domain.Models.Bands.ValueObjects;
using RftmAPI.Domain.Models.Genres;
using RftmAPI.Domain.Models.Genres.ValueObjects;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Bands;

/// <summary>
/// Музыкальная группа.
/// </summary>
public sealed class Band : AggregateRoot<BandId, Guid>
{
    private readonly List<AlbumId> _albumIds = new();
    private readonly List<GenreId> _genreIds = new();

    /// <summary>
    /// Название музыкальной группы.
    /// </summary>
    public BandName Name { get; private set; }

    /// <summary>
    /// Музыкальные альбомы.
    /// </summary>
    public IReadOnlyCollection<AlbumId> AlbumIds => _albumIds;

    /// <summary>
    /// Музыкальные жанры.
    /// </summary>
    public IReadOnlyCollection<GenreId> GenreIds => _genreIds;

    /// <summary>
    /// Создание музыкальной группы.
    /// </summary>
#pragma warning disable CS8618
    private Band()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Создание музыкальной группы.
    /// </summary>
    /// <param name="name">Название музыкальной группы.</param>
    private Band(BandName name) : base(BandId.Create())
    {
        Name = name;
    }

    /// <summary>
    /// Создание музыкальной группы.
    /// </summary>
    /// <param name="name">Название музыкальной группы.</param>
    /// <param name="albums">Музыкальные альбомы.</param>
    /// <param name="genres">Музыкальные жанры.</param>
    private Band(BandName name, IEnumerable<Album> albums, IEnumerable<Genre> genres) : base(BandId.Create())
    {
        Name = name;
        _albumIds = albums.Select(album => (AlbumId) album.Id).ToList();
        _genreIds = genres.Select(genre => (GenreId) genre.Id).ToList();
    }

    /// <summary>
    /// Создание музыкальной группы.
    /// </summary>
    /// <param name="name">Название музыкальной группы.</param>
    /// <returns>Музыкальная группа.</returns>
    public static Result<Band> Create(BandName name)
    {
        return new Band(name);
    }

    /// <summary>
    /// Создание музыкальной группы.
    /// </summary>
    /// <param name="name">Название музыкальной группы.</param>
    /// <param name="albums">Музыкальные альбомы.</param>
    /// <param name="genres">Музыкальные жанры.</param>
    /// <returns>Музыкальная группа.</returns>
    public static Result<Band> Create(BandName name, IEnumerable<Album> albums, IEnumerable<Genre> genres)
    {
        return new Band(name, albums, genres);
    }
}