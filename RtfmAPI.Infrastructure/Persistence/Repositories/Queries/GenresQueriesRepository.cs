using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using RtfmAPI.Application.Interfaces.Persistence.Queries;
using RtfmAPI.Domain.Models.Genres;
using RtfmAPI.Domain.Models.Genres.ValueObjects;
using RtfmAPI.Domain.Primitives;
using RtfmAPI.Infrastructure.Daos;
using RtfmAPI.Infrastructure.Persistence.Context;

namespace RtfmAPI.Infrastructure.Persistence.Repositories.Queries;

/// <summary>
/// Репозиторий запросов доменной модели <see cref="Genre"/>.
/// </summary>
public class GenresQueriesRepository : IGenresQueriesRepository
{
    private readonly DataContext _dataContext;

    /// <summary>
    /// Создание репозитория запросов доменной модели <see cref="Genre"/>.
    /// </summary>
    /// <param name="dataContext">Контекст базы данных.</param>
    public GenresQueriesRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    /// <inheritdoc />
    public async Task<Result<Genre>> GetGenreByIdAsync(GenreId genreId)
    {
        using var connection = _dataContext.CreateOpenedConnection();

        var genre = await GetGenreDaoAsync(genreId.Value, connection);
        if (genre is null)
        {
            return new InvalidOperationException();
        }

        var genresFabric = new GenresFabric(genre.Name);
        return genresFabric.Restore(genre.Id);
    }

    /// <inheritdoc />
    public Task<bool> IsGenreExistsAsync(GenreId genreId)
    {
        var connection = _dataContext.CreateOpenedConnection();
        const string sql = @"SELECT EXISTS(SELECT 1 FROM Genres WHERE Id=@GenreId)";

        return connection.ExecuteScalarAsync<bool>(sql, new {GenreId = genreId.Value});
    }

    /// <summary>
    /// Получение объекта доступа данных музыкального жанра.
    /// </summary>
    /// <param name="genreId">Идентификатор музыкального жанра.</param>
    /// <param name="connection">Соединение.</param>
    /// <returns>Объект доступа данных музыкального жанра.</returns>
    private static Task<GenreDao?> GetGenreDaoAsync(Guid genreId, IDbConnection connection)
    {
        const string sql = @"SELECT * FROM Genres WHERE Id = @GenreId";
        return connection.QuerySingleOrDefaultAsync<GenreDao>(sql, new {GenreId = genreId});
    }
}