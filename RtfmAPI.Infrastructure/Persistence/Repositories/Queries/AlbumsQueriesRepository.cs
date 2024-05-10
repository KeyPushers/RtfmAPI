using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using FluentResults;
using RtfmAPI.Application.Fabrics.Albums;
using RtfmAPI.Application.Fabrics.Albums.Dao;
using RtfmAPI.Application.Interfaces.Persistence.Queries;
using RtfmAPI.Domain.Models.Albums;
using RtfmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Domain.Models.Tracks.ValueObjects;
using RtfmAPI.Infrastructure.Persistence.Context;

namespace RtfmAPI.Infrastructure.Persistence.Repositories.Queries;

/// <summary>
/// Репозиторий запросов доменной модели <see cref="Album"/>.
/// </summary>
public class AlbumsQueriesRepository : IAlbumsQueriesRepository
{
    private readonly DataContext _dataContext;

    /// <summary>
    /// Создание репозитория запросов доменной модели <see cref="Album"/>.
    /// </summary>
    /// <param name="dataContext">Контекст базы данных.</param>
    public AlbumsQueriesRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    /// <inheritdoc />
    public async Task<Result<Album>> GetAlbumByIdAsync(AlbumId albumId)
    {
        var connection = _dataContext.CreateOpenedConnection();

        const string sql = @"SELECT a.*, at.album_id FROM albums a 
            LEFT JOIN albums_tracks at on a.id = at.album_id 
            WHERE a.id = @Id";
        var albumDao = await QuerySingleOrDefaultAsync(connection, sql, new {Id = albumId.Value});
        if (albumDao is null)
        {
            throw new NotImplementedException();
        }

        return RestoreAlbumFromDao(albumDao);
    }

    /// <inheritdoc />
    public async Task<Result<bool>> IsAlbumExistsAsync(AlbumId albumId)
    {
        var connection = _dataContext.CreateOpenedConnection();
        const string sql = @"SELECT EXISTS(SELECT 1 FROM Albums WHERE Id=@AlbumId)";

        return await connection.ExecuteScalarAsync<bool>(sql, new {AlbumId = albumId.Value});
    }

    /// <inheritdoc />
    public async Task<Result<List<Album>>> GetAlbumsByTrackIdAsync(TrackId trackId, CancellationToken cancellationToken)
    {
        var connection = _dataContext.CreateOpenedConnection();

        const string sql = @"SELECT a.*, at.album_id FROM albums a 
            LEFT JOIN albums_tracks at on a.id = at.album_id 
            WHERE at.track_id = @Id";
        var albumDaos = await QueryAsync(connection, sql, new {Id = trackId.Value});

        List<Album> albums = new();
        foreach (var albumDao in albumDaos)
        {
            var restoreAlbumResult = RestoreAlbumFromDao(albumDao);
            if (restoreAlbumResult.IsFailed)
            {
                return restoreAlbumResult.ToResult();
            }

            albums.Add(restoreAlbumResult.ValueOrDefault);
        }

        return albums;
    }

    /// <summary>
    /// Запрос.
    /// </summary>
    /// <param name="connection">Соединение.</param>
    /// <param name="sql">Описание запроса.</param>
    /// <param name="param">Параметр фильтрации.</param>
    /// <returns>Объект доступа данных музыкального альбома.</returns>
    private static async Task<List<AlbumDao>> QueryAsync(IDbConnection connection, string sql,
        object? param = null)
    {
        var queryResult = await connection.QueryAsync<AlbumDao, Guid?, AlbumDao>(sql,
            (album, trackId) =>
            {
                if (trackId is not null)
                {
                    album.TrackIds.Add(trackId.Value);
                }

                return album;
            }, param);

        return queryResult
            .GroupBy(entity => entity.Id)
            .Select(entity =>
            {
                var album = entity.First();
                album.TrackIds = entity
                    .Where(item => item.TrackIds.Any())
                    .Select(item => item.TrackIds.Single())
                    .ToList();
                return album;
            }).ToList();
    }

    /// <summary>
    /// Запрос.
    /// </summary>
    /// <param name="connection">Соединение.</param>
    /// <param name="sql">Описание запроса.</param>
    /// <param name="param">Параметр фильтрации.</param>
    /// <returns>Объект доступа данных музыкального альбома.</returns>
    private static async Task<AlbumDao?> QuerySingleOrDefaultAsync(IDbConnection connection, string sql,
        object? param = null)
    {
        var items = await QueryAsync(connection, sql, param);
        return items.SingleOrDefault();
    }

    /// <summary>
    /// Восстановление музыкального альбома из объекта доступа данных.
    /// </summary>
    /// <param name="albumDao">Объект доступа данных музыкального альбома.</param>
    /// <returns>Музыкальный альбом.</returns>
    private static Result<Album> RestoreAlbumFromDao(AlbumDao albumDao)
    {
        return new AlbumsFactory(albumDao).Restore();
    }
}