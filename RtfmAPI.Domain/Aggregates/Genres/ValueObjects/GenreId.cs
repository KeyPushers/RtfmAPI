using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Aggregates.Genres.ValueObjects;

/// <summary>
/// Идентификатор жанра
/// </summary>
public sealed class GenreId : AggregateRootId<Guid>
{
    public override Guid Value { get; }

    /// <summary>
    /// Идентификатор жнара
    /// </summary>
    private GenreId()
    {
        
    }
    
    /// <summary>
    /// Идентификатор музыкальной группы
    /// </summary>
    /// <param name="value">Значение идентификатор</param>
    private GenreId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Создание идентификатора жанра
    /// </summary>
    /// <returns>Идентификатор жанра</returns>
    public static GenreId Create()
    {
        return new GenreId(Guid.NewGuid());
    }

    /// <summary>
    /// Создание идентификатора жанра
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <returns>Идентификатор жанра</returns>
    public static GenreId Create(Guid id)
    {
        return new GenreId(id);
    }
}