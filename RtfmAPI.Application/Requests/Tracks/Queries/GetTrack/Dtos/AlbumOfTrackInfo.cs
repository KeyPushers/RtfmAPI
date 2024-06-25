using System;

namespace RtfmAPI.Application.Requests.Tracks.Queries.GetTrack.Dtos;

/// <summary>
/// Объект переноса данных информации об альбоме в музыкальном треке.
/// </summary>
public sealed class AlbumOfTrackInfo
{
    /// <summary>
    /// Идентификатор музыкального альбома.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Название музыкального альбома.
    /// </summary>
    public string? Name { get; init; }
}