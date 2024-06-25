using System;

namespace RtfmAPI.Infrastructure.Daos;

/// <summary>
/// Объект доступа данных музыкального жанра.
/// </summary>
public class GenreDao
{
    /// <summary>
    /// Идентификатор музыкального жанра.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Название музыкального жанра.
    /// </summary>
    public string Name { get; set; } = null!;
}