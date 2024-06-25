using System;
using System.Collections.Generic;

namespace RtfmAPI.Infrastructure.Daos;

/// <summary>
/// Объект доступа данных музыкального альбома.
/// </summary>
public class AlbumDao
{
    /// <summary>
    /// Идентификатор музыкального альбома.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Название музыкального альбома.
    /// </summary>
    public string Name { get; set; } = null!;
    
    /// <summary>
    /// Дата выпуска музыкального альбома.
    /// </summary>
    public DateTime ReleaseDate { get; set; }

    /// <summary>
    /// Идентификаторы музыкальных треков.
    /// </summary>
    public List<Guid> TrackIds { get; set; } = new();
}