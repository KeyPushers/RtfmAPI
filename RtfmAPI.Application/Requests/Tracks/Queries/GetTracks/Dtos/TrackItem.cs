using System;
using System.Collections.Generic;

namespace RtfmAPI.Application.Requests.Tracks.Queries.GetTracks.Dtos;

/// <summary>
/// Объект переноса данных музыкального трека.
/// </summary>
public sealed class TrackItem
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
    /// Название музыкальных групп, исполняющих трек.
    /// </summary>
    public List<string> BandNames { get; init; } = new();

    /// <summary>
    /// Продолжительность мзуыкального трека.
    /// </summary>
    public double Duration { get; init; }
}