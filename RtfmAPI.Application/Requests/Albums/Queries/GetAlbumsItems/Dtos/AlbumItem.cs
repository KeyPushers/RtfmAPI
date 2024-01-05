using System;

namespace RtfmAPI.Application.Requests.Albums.Queries.GetAlbumsItems.Dtos;

/// <summary>
/// Объект переноса данных музыкального альбома.
/// </summary>
public class AlbumItem
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Название музыкального альбома.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Дата выпуска музыкального альбома.
    /// </summary>
    public DateTime ReleaseDate { get; set; }
}