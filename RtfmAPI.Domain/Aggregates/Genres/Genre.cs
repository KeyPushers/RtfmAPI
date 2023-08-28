using RftmAPI.Domain.Aggregates.Bands.ValueObjects;
using RftmAPI.Domain.Aggregates.Genres.ValueObjects;
using RftmAPI.Domain.Aggregates.Tracks.ValueObjects;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Aggregates.Genres;

/// <summary>
/// Жанр музыки
/// </summary>
public sealed class Genre : AggregateRoot<GenreId, Guid>
{
    private readonly List<TrackId> _tracks;
    private readonly List<BandId> _bands;

    /// <summary>
    /// Название жанра музыки
    /// </summary>
    public GenreName Name { get; private set; }
    
    /// <summary>
    /// Музыкальные треки, относящиеся к этому жанру
    /// </summary>
    public IEnumerable<TrackId> Tracks => _tracks;
    
    /// <summary>
    /// Музыкальные группы, относящиеся к этому жанру
    /// </summary>
    public IEnumerable<BandId> Bands => _bands;

    /// <summary>
    /// Жанр музыки
    /// </summary>
    /// <param name="name">Название жанра музыки</param>
    public Genre(string name) : base(GenreId.Create())
    {
        Name = new GenreName(name);
        
        _tracks = new List<TrackId>();
        _bands = new List<BandId>();
    }
    
    /// <summary>
    /// Добавление музыкального трека
    /// </summary>
    /// <param name="trackId">Идентификатор музыкального трека</param>
    public void AddTrack(TrackId trackId)
    {
        _tracks.Add(trackId);
    }
    
    /// <summary>
    /// Добавление музыкальной группы
    /// </summary>
    /// <param name="bandId">Идентификатор музыкальной группы</param>
    public void AddBand(BandId bandId)
    {
        _bands.Add(bandId);
    }
}