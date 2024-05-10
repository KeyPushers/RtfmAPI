using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using FluentResults;
using RtfmAPI.Application.Fabrics.TrackFiles;
using RtfmAPI.Application.Fabrics.TrackFiles.Daos;
using RtfmAPI.Application.Interfaces.Persistence.Queries;
using RtfmAPI.Domain.Models.TrackFiles;
using RtfmAPI.Domain.Models.TrackFiles.ValueObjects;
using RtfmAPI.Infrastructure.Persistence.Context;

namespace RtfmAPI.Infrastructure.Persistence.Repositories.Queries;

/// <summary>
/// Репозиторий запросов доменной модели <see cref="TrackFile"/>.
/// </summary>
public class TrackFilesQueriesRepository : ITrackFilesQueriesRepository
{
    private readonly DataContext _context;

    /// <summary>
    /// Создание репозитория запросов доменной модели <see cref="TrackFile"/>.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public TrackFilesQueriesRepository(DataContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<Result<TrackFile>> GetTrackFileByIdAsync(TrackFileId trackFileId)
    {
        var connection = _context.CreateOpenedConnection();
        
        const string sql = @"SELECT * From trackfiles t WHERE t.id = @Id";

        var trackFileDao = await QuerySingleOrDefaultAsync(connection, sql, new {Id = trackFileId.Value});
        if (trackFileDao is null)
        {
            throw new NotImplementedException();
        }

        return RestoreTrackFileFromDao(trackFileDao);
    }

    /// <inheritdoc />
    public async Task<Result<bool>> IsTrackFileExistsAsync(TrackFileId trackFileId)
    {
        var connection = _context.CreateOpenedConnection();
        const string sql = @"SELECT EXISTS(SELECT 1 FROM trackfiles t WHERE t.id = @TrackFileId)";

        return await connection.ExecuteScalarAsync<bool>(sql, new {TrackFileId = trackFileId.Value});
    }

    /// <inheritdoc />
    public async Task<Result<double>> GetTrackFileDurationAsync(TrackFileId trackFileId)
    {
        var connection = _context.CreateOpenedConnection();
        
        const string sql = @"SELECT t.duration From trackfiles t WHERE t.id = @Id";
        var trackFileDuration = await connection.QuerySingleOrDefaultAsync<double>(sql, new {Id = trackFileId.Value});
        return trackFileDuration;
    }
    
    /// <summary>
    /// Запрос.
    /// </summary>
    /// <param name="connection">Соединение.</param>
    /// <param name="sql">Описание запроса.</param>
    /// <param name="param">Параметр фильтрации.</param>
    /// <returns>Объект доступа данных музыкального альбома.</returns>
    private static async Task<List<TrackFileDao>> QueryAsync(IDbConnection connection, string sql, object? param = null)
    {
        var queryResult = await connection.QueryAsync<TrackFileDao>(sql, param);
        return queryResult.ToList();
    }

    /// <summary>
    /// Запрос.
    /// </summary>
    /// <param name="connection">Соединение.</param>
    /// <param name="sql">Описание запроса.</param>
    /// <param name="param">Параметр фильтрации.</param>
    /// <returns>Объект доступа данных музыкального альбома.</returns>
    private static async Task<TrackFileDao?> QuerySingleOrDefaultAsync(IDbConnection connection, string sql,
        object? param = null)
    {
        var items = await QueryAsync(connection, sql, param);
        return items.SingleOrDefault();
    }

    /// <summary>
    /// Восстановление файла музыкального трека из объекта доступа данных.
    /// </summary>
    /// <param name="trackFileDao">Объект доступа данных файла музыкального трека.</param>
    /// <returns>Файл музыкального трека.</returns>
    private static Result<TrackFile> RestoreTrackFileFromDao(TrackFileDao trackFileDao)
    {
        return new TrackFilesFactory(trackFileDao).Restore();
    }
}