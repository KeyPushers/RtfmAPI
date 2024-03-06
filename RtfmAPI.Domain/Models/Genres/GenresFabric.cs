using System;
using RtfmAPI.Domain.Models.Genres.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Genres;

/// <summary>
/// Фабрика музыкальных жанров.
/// </summary>
public class GenresFabric
{
    /// <summary>
    /// Восстановление музыкального жанра.
    /// </summary>
    /// <param name="id">Идентификатор музыкальной группы.</param>
    /// <param name="name">Название музыкальной группы.</param>
    /// <returns>Музыкальная группа.</returns>
    public Result<Genre> Restore(Guid id, string name)
    {
        var genreId = GenreId.Create(id);

        var getGenreNameResult = GenreName.Create(name);
        if (getGenreNameResult.IsFailed)
        {
            return getGenreNameResult.Error;
        }

        var genreName = getGenreNameResult.Value;

        return Genre.Restore(genreId, genreName);
    }
}