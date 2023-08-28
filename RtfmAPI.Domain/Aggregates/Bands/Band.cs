using RftmAPI.Domain.Aggregates.Albums.ValueObjects;
using RftmAPI.Domain.Aggregates.Bands.ValueObjects;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Aggregates.Bands;

/// <summary>
/// Музыкальная группа.
/// Может состоять из одного человека
/// </summary>
public sealed class Band : AggregateRoot<BandId, Guid>
{
    private readonly List<AlbumId> _albums;

    /// <summary>
    /// Альбомы
    /// </summary>
    public IEnumerable<AlbumId> Albums => _albums;
    
    /// <summary>
    /// Имя музыкальной группы
    /// </summary>
    public BandName Name { get; private set; }

    /// <summary>
    /// Музыкальная группа
    /// </summary>
    /// <param name="name">Имя музыкальной группы</param>
    public Band(string name) : base(BandId.Create())
    {
        Name = new BandName(name);

        _albums = new List<AlbumId>();
    }
    
    /// <summary>
    /// Добавление альбома
    /// </summary>
    /// <param name="albumId">Идентификатор альбома</param>
    public void AddAlbum(AlbumId albumId)
    {
        _albums.Add(albumId);
    }
}