using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using FluentResults;
using RtfmAPI.Application.Fabrics.Bands;
using RtfmAPI.Application.Fabrics.Bands.Daos;
using RtfmAPI.Application.Interfaces.Persistence.Queries;
using RtfmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Domain.Models.Bands;
using RtfmAPI.Domain.Models.Bands.ValueObjects;
using RtfmAPI.Infrastructure.Persistence.Context;

namespace RtfmAPI.Infrastructure.Persistence.Repositories.Queries;

/// <summary>
/// Репозиторий запросов доменной модели <see cref="Band"/>.
/// </summary>
public class BandsQueriesRepository : IBandsQueriesRepository
{
    private readonly DataContext _dataContext;

    /// <summary>
    /// Создание репозитория запросов доменной модели <see cref="Band"/>.
    /// </summary>
    /// <param name="dataContext">Контекст базы данных.</param>
    public BandsQueriesRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    /// <inheritdoc />
    public async Task<Result<Band>> GetBandByIdAsync(BandId bandId)
    {
        var connection = _dataContext.CreateOpenedConnection();

        const string sql = @"SELECT b.*, ba.album_id, bg.genre_id FROM bands b
            LEFT JOIN bands_albums ba on b.id = ba.band_id
            LEFT JOIN bands_genres bg on b.id = bg.band_id
            WHERE b.id = @Id";
        var bandDao = await QuerySingleOrDefaultAsync(connection, sql, new {Id = bandId.Value});
        if (bandDao is null)
        {
            throw new NotImplementedException();
        }

        return RestoreBandFromDao(bandDao);
    }

    /// <inheritdoc />
    public async Task<Result<List<Band>>> GetBandsByAlbumIdAsync(AlbumId albumId,
        CancellationToken cancellationToken = default)
    {
        var connection = _dataContext.CreateOpenedConnection();

        const string sql = @"SELECT b.*, ba.album_id, bg.genre_id FROM bands b
            LEFT JOIN bands_albums ba on b.id = ba.band_id
            LEFT JOIN bands_genres bg on b.id = bg.band_id
            WHERE ba.album_id = @Id";
        var bandDaos = await QueryAsync(connection, sql, new {Id = albumId.Value});

        List<Band> bands = new();
        foreach (var bandDao in bandDaos)
        {
            var restoreBandResult = RestoreBandFromDao(bandDao);
            if (restoreBandResult.IsFailed)
            {
                return restoreBandResult.ToResult();
            }

            bands.Add(restoreBandResult.ValueOrDefault);
        }

        return bands;
    }

    /// <summary>
    /// Запрос.
    /// </summary>
    /// <param name="connection">Соединение.</param>
    /// <param name="sql">Описание запроса.</param>
    /// <param name="param">Параметр фильтрации.</param>
    /// <returns>Объект доступа данных музыкального альбома.</returns>
    private static async Task<List<BandDao>> QueryAsync(IDbConnection connection, string sql,
        object? param = null)
    {
        var queryResult = await connection.QueryAsync<BandDao, Guid?, Guid?, BandDao>(sql,
            (band, albumId, genreId) =>
            {
                if (albumId is not null)
                {
                    band.AlbumIds.Add(albumId.Value);
                }

                if (genreId is not null)
                {
                    band.GenreIds.Add(genreId.Value);
                }

                return band;
            }, param);

        return queryResult
            .GroupBy(entity => entity.Id)
            .Select(entity =>
            {
                var album = entity.First();
                album.AlbumIds = entity
                    .Where(item => item.AlbumIds.Any())
                    .Select(item => item.AlbumIds.Single())
                    .ToList();
                album.GenreIds = entity
                    .Where(item => item.GenreIds.Any())
                    .Select(item => item.GenreIds.Single())
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
    private static async Task<BandDao?> QuerySingleOrDefaultAsync(IDbConnection connection, string sql,
        object? param = null)
    {
        var items = await QueryAsync(connection, sql, param);
        return items.SingleOrDefault();
    }

    /// <summary>
    /// Восстановление музыкальной группы из объекта доступа данных.
    /// </summary>
    /// <param name="bandDao">Объект доступа данных музыкальной группы.</param>
    /// <returns>Музыкальная группа.</returns>
    private static Result<Band> RestoreBandFromDao(BandDao bandDao)
    {
        return new BandsFactory(bandDao).Restore();
    }
}