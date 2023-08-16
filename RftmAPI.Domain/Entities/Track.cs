using System.Reflection.Metadata;

namespace RftmAPI.Domain.Entities;

/// <summary>
/// Музыкальный трек
/// </summary>
public class Track : BaseEntity
{
    /// <summary>
    /// Жанры трека
    /// </summary>
    public List<Genre> Genres { get; init; } = new();
    
    /// <summary>
    /// Музыкальные группы
    /// </summary>
    public List<Band> Bands { get; init; } = new();
    
    /// <summary>
    /// Альбом
    /// </summary>
    public Album? Album { get; init; }

    /// <summary>
    /// Музыкальный трек в виде байт
    /// </summary>
    public Blob Data { get; init; }
    
    /// <summary>
    /// Наименование трека
    /// </summary>
    public string? Name { get; init; }
    
    /// <summary>
    /// Дата выпуска трека
    /// </summary>
    public DateTime ReleaseDate { get; init; }
}