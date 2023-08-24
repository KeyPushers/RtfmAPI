using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Aggregates.Genres;

/// <summary>
/// Жанр музыки
/// </summary>
public sealed class Genre : AggregateRoot
{
    private readonly List<Guid> _tracksIds;
    private readonly List<Guid> _bandsIds;

    /// <summary>
    /// Название жанра музыки
    /// </summary>
    public string? Name { get; private set; }
    
    /// <summary>
    /// Музыкальные треки, относящиеся к этому жанру
    /// </summary>
    public IEnumerable<Guid> TracksIds => _tracksIds;
    
    /// <summary>
    /// Музыкальные группы, относящиеся к этому жанру
    /// </summary>
    public IEnumerable<Guid> BandsIds => _bandsIds;

    /// <summary>
    /// Жанр музыки
    /// </summary>
    /// <param name="name">Название жанра музыки</param>
    /// <param name="tracks">Музыкальные треки</param>
    /// <param name="bands">Музыкальные группы</param>
    public Genre(string name, IEnumerable<Guid> tracks, IEnumerable<Guid> bands)
    {
        Name = name;
        
        _tracksIds = tracks.ToList();
        _bandsIds = bands.ToList();
    }
}