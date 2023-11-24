using RftmAPI.Domain.Models.Albums;
using RftmAPI.Domain.Models.Albums.ValueObjects;
using RftmAPI.Domain.Models.Bands.Events;
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
    private readonly HashSet<AlbumId> _albumIds = new();
    private readonly HashSet<GenreId> _genreIds = new();

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
        AddDomainEvent(new BandNameChangedDomainEvent(this, name));
    }

    /// <summary>
    /// Создание музыкальной группы.
    /// </summary>
    /// <param name="name">Название музыкальной группы.</param>
    /// <param name="albums">Музыкальные альбомы.</param>
    /// <param name="genres">Музыкальные жанры.</param>
    private Band(BandName name, IReadOnlyCollection<Album> albums, IReadOnlyCollection<Genre> genres) : base(
        BandId.Create())
    {
        Name = name;
        AddDomainEvent(new BandNameChangedDomainEvent(this, name));

        _albumIds = albums.Select(album => (AlbumId) album.Id).ToHashSet();
        AddDomainEvent(new AlbumsAddedToBandDomainEvent(this, albums));

        _genreIds = genres.Select(genre => (GenreId) genre.Id).ToHashSet();
        AddDomainEvent(new GenresAddedToBandDomainEvent(this, genres));
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
    public static Result<Band> Create(BandName name, IReadOnlyCollection<Album> albums,
        IReadOnlyCollection<Genre> genres)
    {
        return new Band(name, albums, genres);
    }

    /// <summary>
    /// Установление названия музыкальной группы.
    /// </summary>
    /// <param name="name">Название музыкальной группы.</param>
    public BaseResult SetName(BandName name)
    {
        if (Name == name)
        {
            return BaseResult.Success();
        }

        Name = name;
        AddDomainEvent(new BandNameChangedDomainEvent(this, name));
        return BaseResult.Success();
    }

    /// <summary>
    /// Добавление музыкальных альбомов.
    /// </summary>
    /// <param name="albums">Добавляемые альбомы.</param>
    public BaseResult AddAlbums(IReadOnlyCollection<Album> albums)
    {
        if (!albums.Any())
        {
            return BaseResult.Success();
        }

        List<Album> addedAlbums = new();
        foreach (var album in albums)
        {
            var albumId = AlbumId.Create(album.Id.Value);
            if (_albumIds.Contains(albumId))
            {
                continue;
            }

            addedAlbums.Add(album);
            _albumIds.Add(albumId);
        }

        AddDomainEvent(new AlbumsAddedToBandDomainEvent(this, addedAlbums));
        return BaseResult.Success();
    }

    /// <summary>
    /// Удаление музыкальных альбомов.
    /// </summary>
    /// <param name="albums">Удаляемые альбомы.</param>
    public BaseResult RemoveAlbums(IReadOnlyCollection<Album> albums)
    {
        if (!albums.Any())
        {
            return BaseResult.Success();
        }

        List<Album> removedAlbums = new();
        foreach (var album in albums)
        {
            var albumId = AlbumId.Create(album.Id.Value);
            if (!_albumIds.Contains(albumId))
            {
                continue;
            }

            removedAlbums.Add(album);
            _albumIds.Remove(albumId);
        }

        AddDomainEvent(new AlbumsRemovedFromBandDomainEvent(this, removedAlbums));
        return BaseResult.Success();
    }

    /// <summary>
    /// Добавление музыкальных жанров.
    /// </summary>
    /// <param name="genres">Добавляемые жанры.</param>
    public BaseResult AddGenres(IReadOnlyCollection<Genre> genres)
    {
        if (!genres.Any())
        {
            return BaseResult.Success();
        }

        List<Genre> addedGenres = new();
        foreach (var genre in genres)
        {
            var genreId = GenreId.Create(genre.Id.Value);
            if (_genreIds.Contains(genreId))
            {
                continue;
            }

            addedGenres.Add(genre);
            _genreIds.Add(genreId);
        }

        AddDomainEvent(new GenresAddedToBandDomainEvent(this, addedGenres));
        return BaseResult.Success();
    }

    /// <summary>
    /// Удаление музыкальных жанров.
    /// </summary>
    /// <param name="genres">Удаляемые жанры.</param>
    public BaseResult RemoveGenres(IReadOnlyCollection<Genre> genres)
    {
        if (!genres.Any())
        {
            return BaseResult.Success();
        }

        List<Genre> removedGenres = new();
        foreach (var genre in genres)
        {
            var genreId = GenreId.Create(genre.Id.Value);
            if (!_genreIds.Contains(genreId))
            {
                continue;
            }

            removedGenres.Add(genre);
            _genreIds.Remove(genreId);
        }

        AddDomainEvent(new GenresRemovedFromBandDomainEvent(this, removedGenres));
        return BaseResult.Success();
    }
}