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
    private readonly GenresFabric _genreFabric;

    /// <summary>
    /// Создание репозитория запросов доменной модели <see cref="Genre"/>.
    /// </summary>
    /// <param name="dataContext">Контекст базы данных.</param>
    /// <param name="genreFabric">Фабрика музыкальных жанров.</param>
    public GenresQueriesRepository(DataContext dataContext, GenresFabric genreFabric)
    {
        _dataContext = dataContext;
        _genreFabric = genreFabric;
    }
    
    /// <inheritdoc />
    public async Task<Result<Genre>> GetGenreByIdAsync(GenreId genreId)
    {
        using var connection = _dataContext.CreateOpenedConnection();
        
        const string sql = @"SELECT * FROM Genres WHERE Id = @GenreId";
        var band = await connection.QuerySingleOrDefaultAsync<GenreDao>(sql, new {GenreId = genreId.Value});
        if (band is null)
        {
            return new InvalidOperationException();
        }
        
        return _genreFabric.Restore(genreId.Value, band.Name);
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