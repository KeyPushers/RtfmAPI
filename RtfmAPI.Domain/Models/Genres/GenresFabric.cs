using System;
using RtfmAPI.Domain.Fabrics;
using RtfmAPI.Domain.Models.Genres.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Genres;

/// <summary>
/// Фабрика музыкальных жанров.
/// </summary>
public class GenresFabric : AggregateFabric<Genre, GenreId, Guid>
{
    private readonly string _name;

    /// <summary>
    /// Создание фабрики.
    /// </summary>
    /// <param name="name">Название жанра.</param>
    public GenresFabric(string name)
    {
        _name = name;
    }

    /// <inheritdoc />
    public override Result<Genre> Create()
    {
        var getGenreNameResult = GenreName.Create(_name);
        if (getGenreNameResult.IsFailed)
        {
            return getGenreNameResult.Error;
        }

        var genreName = getGenreNameResult.Value;

        return Genre.Create(genreName);
    }

    /// <inheritdoc />
    public override Result<Genre> Restore(Guid id)
    {
        var genreId = GenreId.Create(id);

        var getGenreNameResult = GenreName.Create(_name);
        if (getGenreNameResult.IsFailed)
        {
            return getGenreNameResult.Error;
        }

        var genreName = getGenreNameResult.Value;

        return Genre.Restore(genreId, genreName);
    }
}