using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using FluentResults;
using RtfmAPI.Application.Fabrics.Tracks;
using RtfmAPI.Application.Fabrics.Tracks.Daos;
using RtfmAPI.Application.Interfaces.Persistence.Queries;
using RtfmAPI.Domain.Models.Tracks;
using RtfmAPI.Domain.Models.Tracks.ValueObjects;
using RtfmAPI.Infrastructure.Persistence.Context;

namespace RtfmAPI.Infrastructure.Persistence.Repositories.Queries;

/// <summary>
/// Репозиторий запросов доменной модели <see cref="Track"/>.
/// </summary>
public class TracksQueriesRepository : ITracksQueriesRepository
{
    private readonly DataContext _context;

    /// <summary>
    /// Создание репозитория запросов доменной модели <see cref="Track"/>.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public TracksQueriesRepository(DataContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<Result<Track>> GetTrackByIdAsync(TrackId trackId)
    {
        var connection = _context.CreateOpenedConnection();

        const string sql = @"SELECT t.*, tg.genre_id  From tracks t
            LEFT JOIN tracks_genres tg on t.id = tg.track_id
            WHERE t.id = @Id";
        var trackDao = await QuerySingleOrDefaultAsync(connection, sql, new {Id = trackId.Value});
        if (trackDao is null)
        {
            throw new NotImplementedException();
        }

        return RestoreTrackFromDao(trackDao);
    }

    /// <inheritdoc />
    public async Task<Result<bool>> IsTrackExistsAsync(TrackId trackId)
    {
        var connection = _context.CreateOpenedConnection();
        const string sql = @"SELECT EXISTS(SELECT 1 FROM Tracks WHERE Id=@TrackId)";

        return await connection.ExecuteScalarAsync<bool>(sql, new {TrackId = trackId.Value});
    }

    /// <inheritdoc />
    public async Task<Result<List<Track>>> GetTracksAsync(CancellationToken cancellationToken = default)
    {
        var connection = _context.CreateOpenedConnection();

        const string sql = @"SELECT t.*, tg.genre_id  From tracks t
            LEFT JOIN tracks_genres tg on t.id = tg.track_id";
        var trackDaos = await QueryAsync(connection, sql);

        List<Track> result = new();
        foreach (var trackDao in trackDaos)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var getTrackResult = RestoreTrackFromDao(trackDao);
            if (getTrackResult.IsFailed)
            {
                return getTrackResult.ToResult();
            }

            result.Add(getTrackResult.ValueOrDefault);
        }

        return result;
    }

    /// <summary>
    /// Запрос.
    /// </summary>
    /// <param name="connection">Соединение.</param>
    /// <param name="sql">Описание запроса.</param>
    /// <param name="param">Параметр фильтрации.</param>
    /// <returns>Объект доступа данных музыкального альбома.</returns>
    private static async Task<List<TrackDao>> QueryAsync(IDbConnection connection, string sql,
        object? param = null)
    {
        var queryResult = await connection.QueryAsync<TrackDao, Guid?, TrackDao>(sql,
            (track, genreId) =>
            {
                if (genreId is not null)
                {
                    track.GenreIds.Add(genreId.Value);
                }

                return track;
            }, param);

        return queryResult
            .GroupBy(entity => entity.Id)
            .Select(entity =>
            {
                var track = entity.First();
                track.GenreIds = entity
                    .Where(item => item.GenreIds.Any())
                    .Select(item => item.GenreIds.Single())
                    .ToList();

                return track;
            }).ToList();
    }

    /// <summary>
    /// Запрос.
    /// </summary>
    /// <param name="connection">Соединение.</param>
    /// <param name="sql">Описание запроса.</param>
    /// <param name="param">Параметр фильтрации.</param>
    /// <returns>Объект доступа данных музыкального альбома.</returns>
    private static async Task<TrackDao?> QuerySingleOrDefaultAsync(IDbConnection connection, string sql,
        object? param = null)
    {
        var items = await QueryAsync(connection, sql, param);
        return items.SingleOrDefault();
    }

    /// <summary>
    /// Восстановление музыкального трека из объекта доступа данных.
    /// </summary>
    /// <param name="trackDao">Объект доступа данных музыкального трека.</param>
    /// <returns>Музыкальный трек.</returns>
    private static Result<Track> RestoreTrackFromDao(TrackDao trackDao)
    {
        return new TracksFactory(trackDao).Restore();
    }
}