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
        using var connection = _dataContext.CreateOpenedConnection();

        const string sql = @"SELECT Id, Name FROM Bands WHERE Id = @BandId";
        var band = await connection.QuerySingleOrDefaultAsync<BandDao>(sql, new {BandId = bandId.Value});
        if (band is null)
        {
            return new InvalidOperationException();
        }

        const string sqlAlbumIds = @"SELECT ba.AlbumId FROM BandAlbums ba WHERE ba.BandId = @BandId";
        var albumIds = await connection.QueryAsync<Guid>(sqlAlbumIds, new {BandId = bandId.Value});

        const string sqlGenreIds = @"SELECT bg.GenreId FROM BandGenres bg WHERE bg.BandId = @BandId";
        var genreIds = await connection.QueryAsync<Guid>(sqlGenreIds, new {BandId = bandId.Value});

        band.AlbumIds = albumIds.ToList();
        band.GenreIds = genreIds.ToList();
        
        var bandsFabric = new BandsFabric(band.Name, band.AlbumIds, band.GenreIds);
        return bandsFabric.Restore(band.Id);
    }
}