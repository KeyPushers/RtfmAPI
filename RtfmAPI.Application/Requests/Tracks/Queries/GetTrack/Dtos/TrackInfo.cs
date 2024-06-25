using System;
using System.Collections.Generic;

namespace RtfmAPI.Application.Requests.Tracks.Queries.GetTrack.Dtos;

/// <summary>
/// Объект переноса данных информации о музыкальном треке.
/// </summary>
public sealed class TrackInfo
{
    /// <summary>
    /// Идентификатор музыкального трека.
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Название музыкального трека.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Дата выпуска музыкального трека.
    /// </summary>
    public DateTime ReleaseDate { get; init; }

    /// <summary>
    /// Музыкальные альбомы.
    /// </summary>
    public List<AlbumOfTrackInfo> Albums { get; init; } = new();

    /// <summary>
    /// Продолжительность музыкального трека.
    /// </summary>
    public double Duration { get; init; }

    /// <summary>
    /// Список жанров.
    /// </summary>
    public List<GenreOfTrackInfo> Genres { get; init; } = new();
}