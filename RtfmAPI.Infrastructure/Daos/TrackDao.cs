using System;
using System.Collections.Generic;

namespace RtfmAPI.Infrastructure.Daos;

/// <summary>
/// Объект доступа данных музыкального трека.
/// </summary>
public class TrackDao
{
    /// <summary>
    /// Идентификатор музыкального трека.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Название музыкального трека.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Идентификатор файла музыкального трека.
    /// </summary>
    public Guid? TrackFileId { get; set; }

    /// <summary>
    /// Идентификаторы музыкальных жанров.
    /// </summary>
    public List<Guid> GenreIds { get; set; } = new();
}