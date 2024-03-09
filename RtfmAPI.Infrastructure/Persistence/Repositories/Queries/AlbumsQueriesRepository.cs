using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using RtfmAPI.Application.Interfaces.Persistence.Queries;
using RtfmAPI.Domain.Models.Albums;
using RtfmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Domain.Models.Tracks.ValueObjects;
using RtfmAPI.Domain.Primitives;
using RtfmAPI.Infrastructure.Daos;
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
        var albumDao = await GetAlbumDaoAsync(albumId.Value, connection);
        if (albumDao is null)
        {
            return new InvalidOperationException();
        }

        var albumsFabric = new AlbumsFabric(albumDao.Name, albumDao.ReleaseDate, albumDao.TrackIds);
        return albumsFabric.Restore(albumDao.Id);
    }

    /// <inheritdoc />
    public Task<bool> IsAlbumExistsAsync(AlbumId albumId)
    {
        var connection = _dataContext.CreateOpenedConnection();
        const string sql = @"SELECT EXISTS(SELECT 1 FROM Albums WHERE Id=@AlbumId)";

        return connection.ExecuteScalarAsync<bool>(sql, new {AlbumId = albumId.Value});
    }

    /// <inheritdoc />
    public async Task<Result<List<Album>>> GetAlbumsByTrackIdAsync(TrackId trackId, CancellationToken cancellationToken)
    {
        var connection = _dataContext.CreateOpenedConnection();
        const string sqlAlbumIds = @"SELECT AlbumId FROM AlbumTracks WHERE TrackId = @TrackId";
        var albumIds = await connection.QueryAsync<Guid>(sqlAlbumIds, new {TrackId = trackId.Value});

        List<Album> albums = new();
        foreach (var albumId in albumIds)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return new OperationCanceledException(cancellationToken);
            }

            var albumDao = await GetAlbumDaoAsync(albumId, connection);
            if (albumDao is null)
            {
                return new InvalidOperationException();
            }
            
            var albumsFabric = new AlbumsFabric(albumDao.Name, albumDao.ReleaseDate, albumDao.TrackIds);
            var getAlbumResult = albumsFabric.Restore(albumDao.Id);
            if (getAlbumResult.IsFailed)
            {
                return getAlbumResult.Error;
            }

            albums.Add(getAlbumResult.Value);
        }

        return albums;
    }

    /// <summary>
    /// Получение объекта доступа данных музыкального альбома.
    /// </summary>
    /// <param name="albumId">Идентификатор музыкального альбома.</param>
    /// <param name="connection">Соединение.</param>
    /// <returns>Объект доступа данных музыкального альбома.</returns>
    private static async Task<AlbumDao?> GetAlbumDaoAsync(Guid albumId, IDbConnection connection)
    {
        const string sql = @"SELECT * FROM Albums WHERE Id = @AlbumId";
        var albumDao = await connection.QuerySingleOrDefaultAsync<AlbumDao>(sql, new {AlbumId = albumId});
        if (albumDao is null)
        {
            return null;
        }

        const string sqlTrackIds = @"SELECT at.TrackId FROM AlbumTracks at WHERE at.AlbumId = @AlbumId";
        var trackIds = await connection.QueryAsync<Guid>(sqlTrackIds, new {AlbumId = albumId});

        albumDao.TrackIds = trackIds.ToList();

        return albumDao;
    }
}