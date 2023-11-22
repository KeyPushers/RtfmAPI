namespace RtfmAPI.Application.Requests.Bands.Queries.GetBandInfo.Dtos;

/// <summary>
/// Объект переноса данных информации о музыкальной группе.
/// </summary>
public sealed class BandInfo
{
    /// <summary>
    /// Название музыкальной группы.
    /// </summary>
    public string? Name { get; init; }
}