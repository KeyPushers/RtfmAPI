using RftmAPI.Domain.Models.Genres.ValueObjects;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Genres;

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
    /// Создание музыкального жанра.
    /// </summary>
#pragma warning disable CS8618
    private Genre()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Создание музыкального жанра.
    /// </summary>
    /// <param name="name">Название музыкального жанра.</param>
    private Genre(GenreName name) : base(GenreId.Create())
    {
        Name = name;
    }
    
    /// <summary>
    /// Создание музыкального жанра.
    /// </summary>
    /// <param name="name">Название музыкального жанра.</param>
    /// <returns>Музыкальный жанр.</returns>
    public static Result<Genre> Create(GenreName name)
    {
        return new Genre(name);
    }
}