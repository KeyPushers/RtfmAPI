using System;

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
}