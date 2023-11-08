using System;
using System.Collections.Generic;

namespace RtfmAPI.Application.Requests.Tracks.Queries.GetTrack.Dtos;

/// <summary>
/// Объект переноса данных информации о музыкальном треке.
/// </summary>
public class TrackInfo
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
    /// Идентификатор файла музыкального трека.
    /// </summary>
    public Guid FileId { get; init; }

    /// <summary>
    /// Музыкальный альбом.
    /// </summary>
    public AlbumOfTrackInfo? Album { get; init; }

    /// <summary>
    /// Продолжительность музыкального трека.
    /// </summary>
    public double Duration { get; init; }

    /// <summary>
    /// Список жанров.
    /// </summary>
    public List<GenreOfTrackInfo> Genres { get; init; } = new();
}