using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Aggregates.Relations.TracksAlbums;

/// <summary>
/// Идентификатор музыкального трека
/// </summary>
public sealed class TracksAlbumsId : AggregateRootId<Guid>
{
    public override Guid Value { get; }

    /// <summary>
    /// Идентификатор музыкального трека
    /// </summary>
    private TracksAlbumsId()
    {
        
    }
    
    /// <summary>
    /// Идентификатор музыкального трека
    /// </summary>
    /// <param name="value">Значение идентификатор</param>
    private TracksAlbumsId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Создание идентификатора музыкального трека
    /// </summary>
    /// <returns>Идентификатор музыкального трека</returns>
    public static TracksAlbumsId Create()
    {
        return new TracksAlbumsId(Guid.NewGuid());
    }

    /// <summary>
    /// Создание идентификатора музыкального трека
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <returns>Идентификатор музыкального трека</returns>
    public static TracksAlbumsId Create(Guid id)
    {
        return new TracksAlbumsId(id);
    }
}