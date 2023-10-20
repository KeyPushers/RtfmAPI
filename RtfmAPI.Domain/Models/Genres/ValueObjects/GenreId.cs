using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Genres.ValueObjects;

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
    /// <param name="value">Значение идентификатора.</param>
    private GenreId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Создание идентификатора музыкальной жанра.
    /// </summary>
    /// <returns>Идентификатор музыкального жанра.</returns>
    public static GenreId Create()
    {
        return new GenreId(Guid.NewGuid());
    }

    /// <summary>
    /// Создание идентификатора музыкальной жанра.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <returns>Идентификатор музыкального жанра.</returns>
    public static GenreId Create(Guid id)
    {
        return new GenreId(id);
    }
    
    /// <inheritdoc cref="ValueObject.GetEqualityComponents"/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}