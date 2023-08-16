namespace RftmAPI.Domain.Entities;

/// <summary>
/// Музыкальная группа.
/// Может состоять из одного человека
/// </summary>
public class Band : BaseEntity
{
    /// <summary>
    /// Альбомы
    /// </summary>
    public List<Album> Albums { get; init; } = new();
    
    /// <summary>
    /// Жанры к которым относится музыка группы
    /// </summary>
    public List<Genre> Genres { get; init; } = new();
    
    /// <summary>
    /// Имя музыкальной группы
    /// </summary>
    public string? Name { get; init; }
}