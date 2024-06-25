using System;
using System.Collections.Generic;

namespace RtfmAPI.Infrastructure.Daos;

/// <summary>
/// Объект доступа данных музыкальной группы.
/// </summary>
public class BandDao
{
    /// <summary>
    /// Идентификатор музыкальной группы.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Название музыкальной группы.
    /// </summary>
    public string Name { get; set; } = null!;
    
    /// <summary>
    /// Идентификатор музыкальных альбомов.
    /// </summary>
    public List<Guid> AlbumIds { get; set; } = new();
    
    /// <summary>
    /// Идентификаторы музыкальных жанров.
    /// </summary>
    public List<Guid> GenreIds { get; set; } = new();
}