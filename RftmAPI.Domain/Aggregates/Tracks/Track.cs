﻿using System.Reflection.Metadata;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Aggregates.Tracks;

/// <summary>
/// Музыкальный трек
/// </summary>
public sealed class Track : AggregateRoot
{
    private readonly List<Guid> _genres;
    private readonly List<Guid> _albums;

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
    
    /// <summary>
    /// Жанры
    /// </summary>
    public IEnumerable<Guid> Genres => _genres;

    /// <summary>
    /// Альбомы
    /// </summary>
    public IEnumerable<Guid> Albums => _albums;

    /// <summary>
    /// Музыкальный трек
    /// </summary>
    /// <param name="name">Наименование трека</param>
    /// <param name="data">Музыкальный трек в виде байт</param>
    /// <param name="releaseDate">Дата выпуска трека</param>
    /// <param name="genres">Жанры</param>
    /// <param name="albums">Альбомы</param>
    public Track(string name, Blob data, DateTime releaseDate, IEnumerable<Guid> genres, IEnumerable<Guid> albums)
    {
        Name = new TrackName(name);
        Data = data;
        ReleaseDate = releaseDate;

        _genres = genres.ToList();
        _albums = albums.ToList();
    }
}