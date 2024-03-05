using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using RtfmAPI.Application.Interfaces.Persistence.Queries;
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
    private readonly BandsFabric _bandsFabric;

    /// <summary>
    /// Создание репозитория запросов доменной модели <see cref="Band"/>.
    /// </summary>
    /// <param name="dataContext">Контекст базы данных.</param>
    /// <param name="bandsFabric">Фабрика музыкальных групп.</param>
    public BandsQueriesRepository(DataContext dataContext, BandsFabric bandsFabric)
    {
        _dataContext = dataContext;
        _bandsFabric = bandsFabric;
    }

    /// <inheritdoc />
    public async Task<Result<Band>> GetBandByIdAsync(BandId bandId)
    {
        using var connection = _dataContext.CreateOpenedConnection();
        
        const string sql = @"SELECT Id, Name FROM Bands WHERE Id = @BandId";
        var band = await connection.QuerySingleOrDefaultAsync<BandDao>(sql, new {BandId = bandId.Value});
        if (band is null)
        {
            return new InvalidOperationException();
        }
        
        const string sqlAlbumIds = @"
                    SELECT ba.AlbumId FROM Bands b
                    INNER JOIN BandAlbums ba ON b.Id = ba.BandId
                    WHERE b.id = @BandId
                   ";
        var albumIds = await connection.QueryAsync<Guid>(sqlAlbumIds, new {BandId = bandId.Value});
        
        return _bandsFabric.Restore(bandId.Value, band.Name ?? string.Empty, albumIds.ToList());
    }
}