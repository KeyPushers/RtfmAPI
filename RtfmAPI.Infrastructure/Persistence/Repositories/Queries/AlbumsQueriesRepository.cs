using System;
using System.Threading.Tasks;
using Dapper;
using RtfmAPI.Application.Fabrics;
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
    private readonly AlbumsFabric _albumsFabric;

    /// <summary>
    /// Создание репозитория запросов доменной модели <see cref="Album"/>.
    /// </summary>
    /// <param name="dataContext">Контекст базы данных.</param>
    /// <param name="albumsFabric">Фабрика музыкальных альбомов.</param>
    public AlbumsQueriesRepository(DataContext dataContext, AlbumsFabric albumsFabric)
    {
        _dataContext = dataContext;
        _albumsFabric = albumsFabric;
    }
    
    /// <inheritdoc />
    public async Task<Result<Album>> GetAlbumByIdAsync(AlbumId albumId)
    {
        using var connection = _dataContext.CreateOpenedConnection();
        var sql = @"SELECT Id, Name, ReleaseDate FROM Albums WHERE Id = @AlbumId";
        var response = await connection.QueryFirstOrDefaultAsync<AlbumDao>(sql, new {AlbumId = albumId.Value});
        if (response is null)
        {
            return new InvalidOperationException();
        }

        if (string.IsNullOrWhiteSpace(response.Name))
        {
            return new InvalidOperationException();
        }
        
        return _albumsFabric.CreateAlbum(response.Name, response.ReleaseDate);
    }
}