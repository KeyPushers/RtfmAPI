using System;
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
        
        const string sql = @"SELECT * FROM Genres WHERE Id = @GenreId";
        var genre = await connection.QuerySingleOrDefaultAsync<GenreDao>(sql, new {GenreId = genreId.Value});
        if (genre is null)
        {
            return new InvalidOperationException();
        }

        var genresFabric = new GenresFabric(genre.Name);
        return genresFabric.Restore(genre.Id);
    }

    /// <inheritdoc />
    public async Task<bool> IsGenreExistsAsync(GenreId genreId)
    {
        using var connection = _dataContext.CreateOpenedConnection();
        var trx = connection.BeginTransaction();
        var sql = @"SELECT EXISTS(SELECT 1 FROM Genres WHERE Id=@GenreId)";

        var result = await connection.ExecuteScalarAsync<bool>(sql, new {GenreId = genreId.Value}, trx);
        return result;
    }
}