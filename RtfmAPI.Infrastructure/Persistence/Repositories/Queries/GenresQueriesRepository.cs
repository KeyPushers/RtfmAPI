using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using FluentResults;
using RtfmAPI.Application.Fabrics.Genres;
using RtfmAPI.Application.Fabrics.Genres.Daos;
using RtfmAPI.Application.Interfaces.Persistence.Queries;
using RtfmAPI.Domain.Models.Genres;
using RtfmAPI.Domain.Models.Genres.ValueObjects;
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

        const string sql = @"SELECT * FROM Genres WHERE Id = @Id";

        var genreDao = await QuerySingleOrDefaultAsync(connection, sql, new {Id = genreId.Value});
        if (genreDao is null)
        {
            throw new NotImplementedException();
        }

        return RestoreGenreFromDao(genreDao);
    }

    /// <inheritdoc />
    public async Task<Result<bool>> IsGenreExistsAsync(GenreId genreId)
    {
        var connection = _dataContext.CreateOpenedConnection();
        const string sql = @"SELECT EXISTS(SELECT 1 FROM Genres WHERE Id=@GenreId)";

        return await connection.ExecuteScalarAsync<bool>(sql, new {GenreId = genreId.Value});
    }

    /// <summary>
    /// Запрос.
    /// </summary>
    /// <param name="connection">Соединение.</param>
    /// <param name="sql">Описание запроса.</param>
    /// <param name="param">Параметр фильтрации.</param>
    /// <returns>Объект доступа данных музыкального альбома.</returns>
    private static async Task<List<GenreDao>> QueryAsync(IDbConnection connection, string sql,
        object? param = null)
    {
        var queryResult = await connection.QueryAsync<GenreDao>(sql, param);
        return queryResult.ToList();
    }

    /// <summary>
    /// Запрос.
    /// </summary>
    /// <param name="connection">Соединение.</param>
    /// <param name="sql">Описание запроса.</param>
    /// <param name="param">Параметр фильтрации.</param>
    /// <returns>Объект доступа данных музыкального альбома.</returns>
    private static async Task<GenreDao?> QuerySingleOrDefaultAsync(IDbConnection connection, string sql,
        object? param = null)
    {
        var items = await QueryAsync(connection, sql, param);
        return items.SingleOrDefault();
    }

    /// <summary>
    /// Восстановление музыкального жанра из объекта доступа данных.
    /// </summary>
    /// <param name="genreDao">Объект доступа данных музыкального жанра.</param>
    /// <returns>Музыкальный жанр.</returns>
    private static Result<Genre> RestoreGenreFromDao(GenreDao genreDao)
    {
        return new GenresFactory(genreDao).Restore();
    }
}