using System;
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
        const string trackSql = @"SELECT * From Tracks WHERE Id = @TrackId";
        var trackDao = await connection.QuerySingleOrDefaultAsync<TrackDao>(trackSql, new {TrackId = trackId.Value});
        if (trackDao is null)
        {
            return new InvalidOperationException();
        }

        const string genreIdsSql = @"SELECT tg.GenreId From TrackGenres tg WHERE tg.TrackId = @TrackId";
        var genreIds = await connection.QueryAsync<Guid>(genreIdsSql, new {TrackId = trackId.Value});

        var tracksFabric = new TracksFabric(trackDao.Name ?? string.Empty, trackDao.ReleaseDate, trackDao.TrackFileId, genreIds);
        return tracksFabric.Restore(trackDao.Id);
    }

    /// <inheritdoc />
    public Task<bool> IsTrackExistsAsync(TrackId trackId)
    {
        var connection = _context.CreateOpenedConnection();
        const string sql = @"SELECT EXISTS(SELECT 1 FROM Tracks WHERE Id=@TrackId)";

        return connection.ExecuteScalarAsync<bool>(sql, new {TrackId = trackId.Value});
    }
}