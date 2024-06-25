using System;

namespace RtfmAPI.Application.Requests.Tracks.Queries.GetTrack.Dtos;

/// <summary>
/// Объект переноса данных информации о жанре в музыкалном треке.
/// </summary>
public sealed class GenreOfTrackInfo
{
    /// <summary>
    /// Идентификатор музыкального жанра.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Название музыкального жанра.
    /// </summary>
    public string? Name { get; init; }
}