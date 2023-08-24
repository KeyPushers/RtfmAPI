using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Aggregates.Bands;

/// <summary>
/// Музыкальная группа.
/// Может состоять из одного человека
/// </summary>
public sealed class Band : AggregateRoot
{
    private readonly List<Guid> _albums;

    /// <summary>
    /// Альбомы
    /// </summary>
    public IEnumerable<Guid> Albums => _albums;
    
    /// <summary>
    /// Имя музыкальной группы
    /// </summary>
    public BandName Name { get; private set; }

    /// <summary>
    /// Музыкальная группа
    /// </summary>
    /// <param name="name">Имя музыкальной группы</param>
    /// <param name="albums">Альбомы</param>
    public Band(string name, IEnumerable<Guid> albums)
    {
        Name = new BandName(name);
        
        _albums = albums.ToList();
    }
}