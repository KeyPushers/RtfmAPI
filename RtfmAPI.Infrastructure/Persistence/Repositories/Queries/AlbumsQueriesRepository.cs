using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using RtfmAPI.Application.Interfaces.Persistence.Queries;
using RtfmAPI.Domain.Models.Albums;
using RtfmAPI.Domain.Models.Albums.ValueObjects;
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
        using var connection = _dataContext.CreateOpenedConnection();
        const string sql = @"SELECT Id, Name, ReleaseDate FROM Albums WHERE Id = @AlbumId";
        var response = await connection.QuerySingleOrDefaultAsync<AlbumDao>(sql, new {AlbumId = albumId.Value});
        if (response is null)
        {
            return new InvalidOperationException();
        }

        const string sqlTrackIds = @"SELECT at.TrackId FROM AlbumTracks at WHERE at.AlbumId = @AlbumId";
        var trackIds = await connection.QueryAsync<Guid>(sqlTrackIds, new {AlbumId = albumId.Value});

        response.TrackIds = trackIds.ToList();
        
        var albumsFabric = new AlbumsFabric(response.Name, response.ReleaseDate, response.TrackIds);
        return albumsFabric.Restore(response.Id);
    }

    /// <inheritdoc />
    public async Task<bool> IsAlbumExistsAsync(AlbumId albumId)
    {
        using var connection = _dataContext.CreateOpenedConnection();
        var trx = connection.BeginTransaction();
        const string sql = @"SELECT EXISTS(SELECT 1 FROM Albums WHERE Id=@AlbumId)";

        var result = await connection.ExecuteScalarAsync<bool>(sql, new {AlbumId = albumId.Value}, trx);
        return result;
    }
}