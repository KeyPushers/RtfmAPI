using RftmAPI.Domain.Aggregates.Genre.ValueObjects;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Aggregates.Genre;

/// <summary>
/// Музыкальный жанр.
/// </summary>
public sealed class Genre : AggregateRoot<GenreId, Guid>
{
    /// <summary>
    /// Название музыкального жанра.
    /// </summary>
    public GenreName Name { get; private set; }

    /// <summary>
    /// Музыкальный жанр.
    /// </summary>
#pragma warning disable CS8618
    private Genre()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Музыкальный жанр.
    /// </summary>
    /// <param name="name">Название музыкального жанра.</param>
    public Genre(string name) : base(GenreId.Create())
    {
        Name = new GenreName(name);
    }
}