using FluentResults;
using RtfmAPI.Application.Fabrics.Genres.Daos;
using RtfmAPI.Domain.Models.Genres;
using RtfmAPI.Domain.Models.Genres.ValueObjects;

namespace RtfmAPI.Application.Fabrics.Genres;

/// <summary>
/// Фабрика музыкальных жанров.
/// </summary>
public class GenresFactory : IEntityFactory<Genre, GenreId>
{
    private readonly GenreDao _genreDao;

    /// <summary>
    /// Создание фабрики музыкальных жанров.
    /// </summary>
    /// <param name="genreDao">Объект доступа данных музыкального жанра.</param>
    public GenresFactory(GenreDao genreDao)
    {
        _genreDao = genreDao;
    }

    /// <inheritdoc />
    public Result<Genre> Create()
    {
        var getGenreNameResult = GenreName.Create(_genreDao.Name ?? string.Empty);
        if (getGenreNameResult.IsFailed)
        {
            return getGenreNameResult.ToResult();
        }

        var genreName = getGenreNameResult.ValueOrDefault;

        return Genre.Create(genreName);
    }

    /// <inheritdoc />
    public Result<Genre> Restore()
    {
        var genreId = GenreId.Create(_genreDao.Id);

        var getGenreNameResult = GenreName.Create(_genreDao.Name ?? string.Empty);
        if (getGenreNameResult.IsFailed)
        {
            return getGenreNameResult.ToResult();
        }

        var genreName = getGenreNameResult.ValueOrDefault;

        return Genre.Restore(genreId, genreName);
    }
}