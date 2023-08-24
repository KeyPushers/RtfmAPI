using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Aggregates.Genres;

/// <summary>
/// Жанр музыки
/// </summary>
public sealed class Genre : AggregateRoot
{
    private readonly List<Guid> _tracks;
    private readonly List<Guid> _bands;

    /// <summary>
    /// Название жанра музыки
    /// </summary>
    public GenreName Name { get; private set; }
    
    /// <summary>
    /// Музыкальные треки, относящиеся к этому жанру
    /// </summary>
    public IEnumerable<Guid> Tracks => _tracks;
    
    /// <summary>
    /// Музыкальные группы, относящиеся к этому жанру
    /// </summary>
    public IEnumerable<Guid> Bands => _bands;

    /// <summary>
    /// Жанр музыки
    /// </summary>
    /// <param name="name">Название жанра музыки</param>
    /// <param name="tracks">Музыкальные треки</param>
    /// <param name="bands">Музыкальные группы</param>
    public Genre(string name, IEnumerable<Guid> tracks, IEnumerable<Guid> bands)
    {
        Name = new GenreName(name);
        
        _tracks = tracks.ToList();
        _bands = bands.ToList();
    }
}