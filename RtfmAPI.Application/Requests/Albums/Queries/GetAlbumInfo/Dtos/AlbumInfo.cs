using System;

namespace RtfmAPI.Application.Requests.Albums.Queries.GetAlbumInfo.Dtos;

/// <summary>
/// Объект переноса данных информации о музыкальном альбоме.
/// </summary>
public class AlbumInfo
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