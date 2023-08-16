namespace RftmAPI.Domain.Entities;

/// <summary>
/// Жанр музыки
/// </summary>
public class Genre : BaseEntity
{
    /// <summary>
    /// Название жанра музыки
    /// </summary>
    public string? Name { get; init; }
    
    /// <summary>
    /// Музыкальные треки, относящиеся к этому жанру
    /// </summary>
    public List<Track> Tracks { get; init; } = new();
    
    /// <summary>
    /// Музыкальные группы, относящиеся к этому жанру
    /// </summary>
    public List<Band> Bands { get; init; } = new();
}