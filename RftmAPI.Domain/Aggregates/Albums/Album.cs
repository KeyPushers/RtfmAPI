using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Aggregates.Albums;

/// <summary>
/// Альбом
/// </summary>
public sealed class Album : AggregateRoot
{
    private readonly List<Guid> _tracks;

    /// <summary>
    /// Наименование альбома
    /// </summary>
    public AlbumName Name { get; private set; }

    /// <summary>
    /// Дата выпуска альбома
    /// </summary>
    public DateTime ReleaseDate { get; private set; }

    /// <summary>
    /// Идентификатор музыкальной группы
    /// </summary>
    public Guid BandId { get; private set; }
    
    /// <summary>
    /// Музыкальные треки
    /// </summary>
    public IEnumerable<Guid> Tracks => _tracks;
    
    /// <summary>
    /// Альбом
    /// </summary>
    /// <param name="name">Наименование альбома</param>
    /// <param name="releaseDate">Дата выпуска альбома</param>
    /// <param name="bandId">Идентификатор музыкальной группы</param>
    /// <param name="tracks">Музыкальные треки</param>
    public Album(string name, DateTime releaseDate, Guid bandId, IEnumerable<Guid> tracks)
    {
        Name = new AlbumName(name);
        ReleaseDate = releaseDate;
        BandId = bandId;
        
        _tracks = tracks.ToList();
    }
}