using System;

namespace RtfmAPI.Application.Requests.Bands.Queries.GetBands.Dtos;

/// <summary>
/// Объект переноса данных музыкальной группы.
/// </summary>
public sealed class BandItem
{
    /// <summary>
    /// Идентификатор музыкальной группы.
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Название музыкальной группы.
    /// </summary>
    public string? Name { get; init; }
}