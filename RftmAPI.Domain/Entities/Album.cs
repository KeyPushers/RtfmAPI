namespace RftmAPI.Domain.Entities;

/// <summary>
/// Альбом
/// </summary>
public class Album : BaseEntity
{
    /// <summary>
    /// Музыкальные треки
    /// </summary>
    public List<Track> Tracks { get; init; } = new();
    
    /// <summary>
    /// Музыкальные группы
    /// </summary>
    public List<Band> Bands { get; init; } = new();
    
    /// <summary>
    /// Наименование альбома
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Дата выпуска альбома
    /// </summary>
    public DateTime ReleaseDate { get; init; }
}