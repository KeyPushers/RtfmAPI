using System;
using System.Collections.Generic;
using System.Linq;
using FluentResults;
using RtfmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Domain.Models.Bands.Events;
using RtfmAPI.Domain.Models.Bands.ValueObjects;
using RtfmAPI.Domain.Models.Genres.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Bands;

/// <summary>
/// Музыкальная группа.
/// </summary>
public sealed class Band : AggregateRoot<BandId, Guid>
{
    private readonly HashSet<AlbumId> _albumIds;
    private readonly HashSet<GenreId> _genreIds;

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
    /// <param name="name">Название музыкальной группы.</param>
    /// <param name="albumIds">Идентификаторы музыкальных альбомов.</param>
    /// <param name="genreIds">Идентификаторы музыкальных жанров.</param>
    private Band(BandName name, IEnumerable<AlbumId> albumIds, IEnumerable<GenreId> genreIds) : this(BandId.Create(),
        name, albumIds, genreIds)
    {
    }

    /// <summary>
    /// Создание музыкальной группы.
    /// </summary>
    /// <param name="id">Идентификатор музыкальной группы.</param>
    /// <param name="name">Название музыкальной группы.</param>
    /// <param name="albumIds">Идентификаторы музыкальных альбомов.</param>
    /// <param name="genreIds">Идентификаторы музыкальных жанров.</param>
    private Band(BandId id, BandName name, IEnumerable<AlbumId> albumIds, IEnumerable<GenreId> genreIds) : base(id)
    {
        Name = name;
        _albumIds = albumIds.ToHashSet();
        _genreIds = genreIds.ToHashSet();
    }

    /// <summary>
    /// Создание музыкальной группы.
    /// </summary>
    /// <param name="name">Название музыкальной группы.</param>
    /// <returns>Музыкальная группа.</returns>
    public static Result<Band> Create(BandName name)
    {
        var band = new Band(name, Enumerable.Empty<AlbumId>(), Enumerable.Empty<GenreId>());
        band.AddDomainEvent(new BandCreatedDomainEvent(band));
        band.AddDomainEvent(new BandNameChangedDomainEvent(band, band.Name));
        band.AddDomainEvent(new AlbumsAddedToBandDomainEvent(band, band.AlbumIds));
        band.AddDomainEvent(new GenresAddedToBandDomainEvent(band, band.GenreIds));

        return band;
    }

    /// <summary>
    /// Восстановление музыкальной группы.
    /// </summary>
    /// <param name="id">Идентификатор музыкальной группы.</param>
    /// <param name="name">Название музыкальной группы.</param>
    /// <param name="albumIds">Идентификаторы альбомов музыкальной группы.</param>
    /// <param name="genreIds">Идентификаторы жанров музыкальной группы.</param>
    public static Result<Band> Restore(BandId id, BandName name, IEnumerable<AlbumId> albumIds,
        IEnumerable<GenreId> genreIds)
    {
        return new Band(id, name, albumIds.ToList(), genreIds.ToList());
    }

    /// <summary>
    /// Установление названия музыкальной группы.
    /// </summary>
    /// <param name="name">Название музыкальной группы.</param>
    public Result SetName(BandName name)
    {
        if (Name == name)
        {
            return Result.Ok();
        }

        Name = name;
        AddDomainEvent(new BandNameChangedDomainEvent(this, name));
        return Result.Ok();
    }

    /// <summary>
    /// Добавление музыкальных альбомов.
    /// </summary>
    /// <param name="albumIds">Идентификаторы добавляемых альбомов.</param>
    public Result AddAlbums(IReadOnlyCollection<AlbumId> albumIds)
    {
        if (!albumIds.Any())
        {
            return Result.Ok();
        }

        List<AlbumId> addedAlbums = new();
        foreach (var albumId in albumIds)
        {
            if (_albumIds.Contains(albumId))
            {
                continue;
            }

            addedAlbums.Add(albumId);
            _albumIds.Add(albumId);
        }

        AddDomainEvent(new AlbumsAddedToBandDomainEvent(this, addedAlbums));
        return Result.Ok();
    }

    /// <summary>
    /// Удаление музыкальных альбомов.
    /// </summary>
    /// <param name="albumIds">Идентификаторы удаляемых альбомов.</param>
    public Result RemoveAlbums(IReadOnlyCollection<AlbumId> albumIds)
    {
        if (!albumIds.Any())
        {
            return Result.Ok();
        }

        List<AlbumId> removedAlbumIds = new();
        foreach (var albumId in albumIds)
        {
            if (!_albumIds.Contains(albumId))
            {
                continue;
            }

            removedAlbumIds.Add(albumId);
            _albumIds.Remove(albumId);
        }

        AddDomainEvent(new AlbumsRemovedFromBandDomainEvent(this, removedAlbumIds));
        return Result.Ok();
    }

    /// <summary>
    /// Добавление музыкальных жанров.
    /// </summary>
    /// <param name="genreIds">Идентификаторы добавляемых музыкальных жанров.</param>
    public Result AddGenres(IReadOnlyCollection<GenreId> genreIds)
    {
        if (!genreIds.Any())
        {
            return Result.Ok();
        }

        List<GenreId> addedGenreIds = new();
        foreach (var genreId in genreIds)
        {
            if (_genreIds.Contains(genreId))
            {
                continue;
            }

            addedGenreIds.Add(genreId);
            _genreIds.Add(genreId);
        }

        AddDomainEvent(new GenresAddedToBandDomainEvent(this, addedGenreIds));
        return Result.Ok();
    }

    /// <summary>
    /// Удаление музыкальных жанров.
    /// </summary>
    /// <param name="genreIds">Идентификаторы удаляемых музыкальных жанров.</param>
    public Result RemoveGenres(IReadOnlyCollection<GenreId> genreIds)
    {
        if (!genreIds.Any())
        {
            return Result.Ok();
        }

        List<GenreId> removedGenreIds = new();
        foreach (var genreId in genreIds)
        {
            if (!_genreIds.Contains(genreId))
            {
                continue;
            }

            removedGenreIds.Add(genreId);
            _genreIds.Remove(genreId);
        }

        AddDomainEvent(new GenresRemovedFromBandDomainEvent(this, removedGenreIds));
        return Result.Ok();
    }

    /// <summary>
    /// Удаление музыкальной группы.
    /// </summary>
    public Result Delete()
    {
        AddDomainEvent(new BandDeletedDomainEvent(this));
        return Result.Ok();
    }
}