using System.Collections.Generic;

namespace RtfmAPI.Application.Requests.Bands.Queries.GetBands.Dtos;

/// <summary>
/// Объект переноса данных музыкальных групп.
/// </summary>
public sealed class BandItems
{
    /// <summary>
    /// Объекты переноса данных музыкальных групп.
    /// </summary>
    public List<BandItem> Bands { get; init; } = new();
}