using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Aggregates.Genre.ValueObjects;

/// <summary>
/// Идентификатор музыкального жанра.
/// </summary>
public sealed class GenreId : AggregateRootId<Guid>
{
    /// <summary>
    /// Значение идентификатора.
    /// </summary>
    public override Guid Value { get; }
    
    /// <summary>
    /// Идентификатор музыкального жанра.
    /// </summary>
    private GenreId()
    {
        
    }
    
    /// <summary>
    /// Идентификатор музыкального жанра
    /// </summary>
    /// <param name="value">Значение идентификатор</param>
    private GenreId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Создание идентификатора музыкальной жанра
    /// </summary>
    /// <returns>Идентификатор музыкального трека</returns>
    public static GenreId Create()
    {
        return new GenreId(Guid.NewGuid());
    }

    /// <summary>
    /// Создание идентификатора музыкальной жанра
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <returns>Идентификатор музыкального трека</returns>
    public static GenreId Create(Guid id)
    {
        return new GenreId(id);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    
}