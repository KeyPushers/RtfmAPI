using System;
using System.Threading.Tasks;
using Dapper;
using RtfmAPI.Application.Fabrics;
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
        var sql = @"SELECT Id, Name FROM Bands WHERE Id = @BandId";
        var response = await connection.QueryFirstOrDefaultAsync<BandDao>(sql, new {BandId = bandId.Value});
        if (response is null)
        {
            return new InvalidOperationException();
        }

        return _bandsFabric.CreateBand(response.Name ?? string.Empty);
    }
}