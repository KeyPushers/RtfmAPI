using System.Collections.Generic;

namespace RtfmAPI.Application.Requests.Tracks.Queries.GetTracks.Dtos;

/// <summary>
/// Объект переноса данных музыкальных треков.
/// </summary>
public sealed class TrackItems
{
    /// <summary>
    /// Объекты переноса данных музыкальных треков.
    /// </summary>
    public List<TrackItem> Tracks { get; init; } = new();
}