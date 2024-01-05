using System.Collections.Generic;

namespace RtfmAPI.Application.Requests.Albums.Queries.GetAlbumsItems.Dtos;

/// <summary>
/// Объект переноса данных музыкальных альбомов.
/// </summary>
public class AlbumsItems
{
    /// <summary>
    /// Объекты переноса данных музыкальных альбомов.
    /// </summary>
    public List<AlbumItem> Albums { get; init; } = new();
}