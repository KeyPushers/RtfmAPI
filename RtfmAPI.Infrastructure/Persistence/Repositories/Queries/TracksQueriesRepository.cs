using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using RtfmAPI.Application.Interfaces.Persistence.Queries;
using RtfmAPI.Domain.Models.Tracks;
using RtfmAPI.Domain.Models.Tracks.ValueObjects;
using RtfmAPI.Domain.Primitives;
using RtfmAPI.Infrastructure.Daos;
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
        var trackDao = await GetTrackDaoAsync(trackId.Value, connection);
        if (trackDao is null)
        {
            return new InvalidOperationException();
        }

        var tracksFabric = new TracksFabric(trackDao.Name ?? string.Empty, trackDao.ReleaseDate, trackDao.TrackFileId,
            trackDao.GenreIds);
        return tracksFabric.Restore(trackDao.Id);
    }

    /// <inheritdoc />
    public Task<bool> IsTrackExistsAsync(TrackId trackId)
    {
        var connection = _context.CreateOpenedConnection();
        const string sql = @"SELECT EXISTS(SELECT 1 FROM Tracks WHERE Id=@TrackId)";

        return connection.ExecuteScalarAsync<bool>(sql, new {TrackId = trackId.Value});
    }

    /// <inheritdoc />
    public async Task<Result<List<Track>>> GetTracksAsync(CancellationToken cancellationToken = default)
    {
        var connection = _context.CreateOpenedConnection();
        var sql = @"SELECT Id FROM Tracks";
        var trackIds = await connection.QueryAsync<Guid>(sql);

        List<Track> result = new();
        foreach (var trackId in trackIds)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return new OperationCanceledException(cancellationToken);
            }


            var getTrackResult = await GetTrackByIdAsync(TrackId.Create(trackId));
            if (getTrackResult.IsFailed)
            {
                return getTrackResult.Error;
            }

            result.Add(getTrackResult.Value);
        }

        return result;
    }

    /// <summary>
    /// Получение объекта доступа данных музыкального трека.
    /// </summary>
    /// <param name="trackId">Идентификатор музыкального трека.</param>
    /// <param name="connection">Соединение.</param>
    /// <returns>Объект доступа данных музыкального трека.</returns>
    private async Task<TrackDao?> GetTrackDaoAsync(Guid trackId, IDbConnection connection)
    {
        const string trackSql = @"SELECT * From Tracks WHERE Id = @TrackId";
        var trackDao = await connection.QuerySingleOrDefaultAsync<TrackDao>(trackSql, new {TrackId = trackId});
        if (trackDao is null)
        {
            return null;
        }

        const string genreIdsSql = @"SELECT tg.GenreId From TrackGenres tg WHERE tg.TrackId = @TrackId";
        var genreIds = await connection.QueryAsync<Guid>(genreIdsSql, new {TrackId = trackId});

        trackDao.GenreIds = genreIds.ToList();
        return trackDao;
    }
}