using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using RtfmAPI.Application.Interfaces.Persistence.Queries;
using RtfmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Domain.Models.Bands;
using RtfmAPI.Domain.Models.Bands.ValueObjects;
using RtfmAPI.Domain.Primitives;
using RtfmAPI.Infrastructure.Daos;
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
        var bandDao = await GetBandDaoAsync(bandId.Value, connection);
        if (bandDao is null)
        {
            return new InvalidOperationException();
        }
        
        var bandsFabric = new BandsFabric(bandDao.Name, bandDao.AlbumIds, bandDao.GenreIds);
        return bandsFabric.Restore(bandDao.Id);
    }

    /// <inheritdoc />
    public async Task<Result<List<Band>>> GetBandsByAlbumIdAsync(AlbumId albumId, CancellationToken cancellationToken = default)
    {
        var connection = _dataContext.CreateOpenedConnection();
        const string sqlBandIds = @"SELECT BandId FROM BandAlbums WHERE AlbumId = @AlbumId";
        var bandIds = await connection.QueryAsync<Guid>(sqlBandIds, new {AlbumId = albumId.Value});

        List<Band> result = new();
        foreach (var bandId in bandIds)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return new OperationCanceledException(cancellationToken);
            }

            var bandDao = await GetBandDaoAsync(bandId, connection);
            if (bandDao is null)
            {
                return new InvalidOperationException();
            }
            
            var bandsFabric = new BandsFabric(bandDao.Name, bandDao.AlbumIds, bandDao.GenreIds);
            var getBandResult = bandsFabric.Restore(bandDao.Id);
            if (getBandResult.IsFailed)
            {
                return getBandResult.Error;
            }

            result.Add(getBandResult.Value);
        }

        return result;
    }

    /// <summary>
    /// Получение объекта доступа данных музыкальной группы.
    /// </summary>
    /// <param name="bandId">Идентификатор музыкальной группы.</param>
    /// <param name="connection">Соединение.</param>
    /// <returns>Объект доступа данных музыкальной группы.</returns>
    private static async Task<BandDao?> GetBandDaoAsync(Guid bandId, IDbConnection connection)
    {
        const string sql = @"SELECT * FROM Bands WHERE Id = @BandId";
        var band = await connection.QuerySingleOrDefaultAsync<BandDao>(sql, new {BandId = bandId});
        if (band is null)
        {
            return null;
        }

        const string sqlAlbumIds = @"SELECT ba.AlbumId FROM BandAlbums ba WHERE ba.BandId = @BandId";
        var albumIds = await connection.QueryAsync<Guid>(sqlAlbumIds, new {BandId = bandId});

        const string sqlGenreIds = @"SELECT bg.GenreId FROM BandGenres bg WHERE bg.BandId = @BandId";
        var genreIds = await connection.QueryAsync<Guid>(sqlGenreIds, new {BandId = bandId});

        band.AlbumIds = albumIds.ToList();
        band.GenreIds = genreIds.ToList();

        return band;
    }
}